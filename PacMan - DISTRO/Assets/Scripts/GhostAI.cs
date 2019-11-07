using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*****************************************************************************
 * IMPORTANT NOTES - PLEASE READ
 * 
 * This is where all the code needed for the Ghost AI goes. There should not
 * be any other place in the code that needs your attention.
 * 
 * There are several sets of variables set up below for you to use. Some of
 * those settings will do much to determine how the ghost behaves. You don't
 * have to use this if you have some other approach in mind. Other variables
 * are simply things you will find useful, and I am declaring them for you
 * so you don't have to.
 * 
 * If you need to add additional logic for a specific ghost, you can use the
 * variable ghostID, which is set to 1, 2, 3, or 4 depending on the ghost.
 * 
 * Similarly, set ghostID=ORIGINAL when the ghosts are doing the "original" 
 * PacMan ghost behavior, and to CUSTOM for the new behavior that you supply. 
 * Use ghostID and ghostMode in the Update() method to control all this.
 * 
 * You could if you wanted to, create four separate ghost AI modules, one per
 * ghost, instead. If so, call them something like BlinkyAI, PinkyAI, etc.,
 * and bind them to the correct ghost prefabs.
 * 
 * Finally there are a couple of utility routines at the end.
 * 
 * Please note that this implementation of PacMan is not entirely bug-free.
 * For example, player does not get a new screenful of dots once all the
 * current dots are consumed. There are also some issues with the sound 
 * effects. By all means, fix these bugs if you like.
 * 
 *****************************************************************************/

// HOW THE GHOST BEHAVIOR WORKS
// Ghosts Cannot Reverse Directions (If they're traveling vertically, their next direction has to be horizontal. Vice Versa)
// When Ghosts leave the ghost house, they always start moving left
// Ghosts follow a target tile. They all follow it in the same way, but differ in choosing which tile to target
// 3 Modes: Chase, Scatter, and Frightened
// Scatter Mode: Ghosts target separate corners. Red top right, Pink top left, Blue bottom right, Orange bottom left
// Targets in scatter mode are located outside of the board, and since ghosts can't turn around if they were in scatter mode forever they would do loops around the corner obstacles. 
// Frightened Mode: Ghosts make random turn choices at intersections
// Chase Mode: Ghosts choose target tile as specified below
// RED (BLINKY) Target tile is pacman directly
// PINK (PINKY) Target tile is 2 in front of pacman, exits ghost house immediately after red moves off the tile.  
// BLUE (INKY) Target tile is calculated as following: Take Pinky's target tile (2 tiles in front of pac man), calculate the distance from Blinky to it, invert that distance. I'll show a chart below. It exits the ghost house after pacman eats 30 dots
/*
 *    ------I---   O = pacman (facing right)
 *    ----------   B = Blinky
 *    --O-P-----   P = Pinky Target Tile
 *    ----------   I = Inky Target Tile
 *    --B-------   
 */
// ORANGE (CLYDE) Exits ghost house after 1/3 of dots eaten (#?), when he is less than 8 tile distance from pacman, he chases like blinky, otherwise he goes to scatter mode

// Ghosts are in frightened mode when a power pellet is eaten.
// Otherwise, ghosts go between scatter and chase mode. They follow the following timer:
// Scatter 7 sec, Chase 20 sec, Scatter 7 sec, Chase 20 sec, Scatter 5 sec, Chase 20 sec, Scatter 5 sec, Chase permanently
// The timer is paused when in frightened mode, and is reset when starting a new level or losing a life

// No fancy turning AI A* code, simply turn in the direction of the target tile
// We might want to make target tiles child gameobjects of pacman so the unity Transform engine just tracks it all for us

// We need to model the 4 FSM's like the original, and make another version for each. An advanced one could be implementing A* instead of simple turning
// Or we could just tweak the chase values

// Scatter isn't required for the homework, in fact it's only 2% extra credit to implement. 

public class GhostAI : MonoBehaviour {

    const int BLINKY = 1;   // These are used to set ghostID, to facilitate testing.
    const int PINKY = 2;
    const int INKY = 3;
    const int CLYDE = 4;
    public int ghostID;     // This variable is set to the particular ghost in the prefabs,

    const int ORIGINAL = 1; // These are used to set ghostMode, needed for the assignment.
    const int CUSTOM = 2;
    public int ghostMode;   // ORIGINAL for "original" ghost AI; CUSTOM for your unique new AI

    Movement move;
    private Vector3 startPos;
    private bool[] dirs = new bool[4];
	private bool[] prevDirs = new bool[4];

	public float releaseTime = 0f;          // This could be a tunable number
	private float releaseTimeReset = 0f;
	public float waitTime = 0f;             // This could be a tunable number
    private const float ogWaitTime = .1f;
	public int range = 0;                   // This could be a tunable number

    public bool dead = false;               // state variables
	public bool fleeing = false;
    private Vector3 leave1;
    bool foundLeave1 = false;
    private Vector3 leave2;
    public float speed;

    //Default: base value of likelihood of choice for each path
    public float Dflt = 1f;

	//Available: Zero or one based on whether a path is available
	int A = 0;

	//Value: negative 1 or 1 based on direction of pacman
	int V = 1;

	//Fleeing: negative if fleeing
	int F = 1;

	//Priority: calculated preference based on distance of target in one direction weighted by the distance in others (dist/total)
	float P = 0f;

    // Variables to hold distance calcs
	float distX = 0f;
	float distY = 0f;
	float total = 0f;

    // Percent chance of each coice. order is: up < right < 0 < down < left for random choice
    // These could be tunable numbers. You may or may not find this useful.
    public float[] directions = new float[4];
    
	//remember previous choice and make inverse illegal!
	private int[] prevChoices = new int[4]{1,1,1,1};

    // This will be PacMan when chasing, or Gate, when leaving the Pit
	public GameObject target;
	GameObject gate;
	GameObject pacMan;

	public bool chooseDirection = true;
	public int[] choices ;
	public float choice;

	public enum State{
		waiting,
		entering,
		leaving,
		active,
		fleeing,
        scatter         // Optional - This is for more advanced ghost AI behavior
	}

	public State _state = State.waiting;

    public Transform targetTile;
    public Vector3 actualTarget;

    int currentDir = 1;

    private int movementConfirm = 10;

    // Use this for initialization
    private void Awake()
    {
        startPos = this.gameObject.transform.position;
    }

    void Start () {
		move = GetComponent<Movement> ();
        speed = move.MSpeed;
		gate = GameObject.Find("Gate(Clone)");
		pacMan = GameObject.Find("PacMan(Clone)") ? GameObject.Find("PacMan(Clone)") : GameObject.Find("PacMan 1(Clone)");
		releaseTimeReset = releaseTime;
        leave1 = new Vector3(13.5f, -13.9f, -2f);
        leave2 = new Vector3(13.5f, -11.0f, -2f);
	}

	public void restart(){
		releaseTime = releaseTimeReset;
		transform.position = startPos;
		_state = State.waiting;
	}
	
    /// <summary>
    /// This is where most of the work will be done. A switch/case statement is probably 
    /// the first thing to test for. There can be additional tests for specific ghosts,
    /// controlled by the GhostID variable. But much of the variations in ghost behavior
    /// could be controlled by changing values of some of the above variables, like
    /// 
    /// </summary>
	void Update () {
        //if (targetTile.name[0] == 'B') Debug.Log(_state);
        int newDir;
        switch (_state) {
		case(State.waiting):

            // below is some sample code showing how you deal with animations, etc.
			move._dir = Movement.Direction.still;
			if (releaseTime <= 0f) {
				chooseDirection = true;
				gameObject.GetComponent<Animator>().SetBool("Dead", false);
				gameObject.GetComponent<Animator>().SetBool("Running", false);
				gameObject.GetComponent<Animator>().SetInteger ("Direction", 0);
				gameObject.GetComponent<Movement> ().MSpeed = 5f;

				_state = State.leaving;

                // etc.
			}
			gameObject.GetComponent<Animator>().SetBool("Dead", false);
			gameObject.GetComponent<Animator>().SetBool("Running", false);
			gameObject.GetComponent<Animator>().SetInteger ("Direction", 0);
			gameObject.GetComponent<Movement> ().MSpeed = 5f;
			releaseTime -= Time.deltaTime;
            // etc.
			break;


		case(State.leaving):
                //Stuck in this state?
                //TODO: Have The Ghosts transform.translate towards the starting pos, turn state to active when finished

            actualTarget = leave2;
            
            movementConfirm--;
            
            newDir = directionToTurn(dead);
            
            if (newDir != currentDir) {
                if (movementConfirm <= 0) {
                    currentDir = newDir;
                    move._dir = (Movement.Direction)currentDir;
                    movementConfirm = 10;
                }
            }
            else
            {
                actualTarget = leave2;
            }


            transform.position = Vector3.MoveTowards(transform.position, actualTarget, Time.deltaTime*speed);

            if ((leave1 - transform.position).magnitude < 0.0001f && !foundLeave1)
            {
                foundLeave1 = true;
            }

                // When we reach our target (approximately), move on to the next state
                if ((leave2 - transform.position).magnitude < 0.0001f)
            {
                    _state = State.active;
                    move._dir = Movement.Direction.right;
            }
            
			break;

		case(State.active):

            if (fleeing) {

            }
            if (dead) {
                    //I think this is what happens when power pelleted? Move back to ghost house, then set mode to leaving
                    actualTarget = startPos;
                    //If back at starting position
                    if (Vector3.Distance(transform.position, actualTarget) < 0.2f) {
                        _state = State.leaving;
                        gameObject.GetComponent<Animator>().SetBool("Dead", false);
                        gameObject.GetComponent<Animator>().SetBool("Running", false);
                        gameObject.GetComponent<Animator>().SetBool("Flicker", false);
                        dead = false;
                        fleeing = false;
                        move.MSpeed = 5f;
                        gameObject.GetComponent<CircleCollider2D>().enabled = true;
                    }

                } else {
                    actualTarget = targetTile.position;
                }
            //Steering AI here
            movementConfirm--;


             
            if (fleeing) {
                newDir = getFleeingDirection();
            }
            else {
                newDir = directionToTurn(dead);
            } 
            
            if (newDir != currentDir) {
                if (movementConfirm <= 0) {
                    currentDir = newDir;
                    move._dir = (Movement.Direction)currentDir;
                    movementConfirm = 10;
                }
            }

			break;

		case State.entering:

            // Leaving this code in here for you.
			move._dir = Movement.Direction.still;

			if (transform.position.x < 13.48f || transform.position.x > 13.52) {
				//print ("GOING LEFT OR RIGHT");
				transform.position = Vector3.Lerp (transform.position, new Vector3 (13.5f, transform.position.y, transform.position.z), 3f * Time.deltaTime);
			} else if (transform.position.y > -13.99f || transform.position.y < -14.01f) {
				gameObject.GetComponent<Animator>().SetInteger ("Direction", 2);
				transform.position = Vector3.Lerp (transform.position, new Vector3 (transform.position.x, -14f, transform.position.z), 3f * Time.deltaTime);
			} else {
				fleeing = false;
				dead = false;
				gameObject.GetComponent<Animator>().SetBool("Running", true);
				_state = State.waiting;
			}

            break;
		}
	}

    // Utility routines

	Vector2 num2vec(int n){
        switch (n)
        {
            case 0:
                return new Vector2(0, 1);
            case 1:
    			return new Vector2(1, 0);
		    case 2:
			    return new Vector2(0, -1);
            case 3:
			    return new Vector2(-1, 0);
            default:    // should never happen
                return new Vector2(0, 0);
        }
	}

    Vector3 num2vec3(int n) {
        switch (n) {
            case 0:
                return new Vector3(0, 1, 0);
            case 1:
                return new Vector3(1, 0, 0);
            case 2:
                return new Vector3(0, -1, 0);
            case 3:
                return new Vector3(-1, 0, 0);
            default:    // should never happen
                return new Vector3(0, 0, 0);
        }
    }

	bool compareDirections(bool[] n, bool[] p){
		for(int i = 0; i < n.Length; i++){
			if (n [i] != p [i]) {
				return false;
			}
		}
		return true;
	}

    private int directionToTurn(bool isDead) {
        List<int> potentialDirs = new List<int>();
        for (int i = 0; i < 4; i++) {
            //Can't turn around, ignore
            if ((i + 2) % 4 == currentDir) continue;

            if (move.checkDirectionClear(num2vec(i), !isDead)) {
                potentialDirs.Add(i);
            }
        }

        //Loop through directions, calculate which one will make pacman be closer to the target
        float lowestDistance = 999;
        int directionToMove = -1;
        for (int i = 0; i < potentialDirs.Count; i++) {
            float currentDirDistance = Vector3.Distance(transform.position + num2vec3(potentialDirs[i]), actualTarget);
            if (currentDirDistance < lowestDistance) {
                lowestDistance = currentDirDistance;
                directionToMove = potentialDirs[i];
            }
        }

        return directionToMove;
    }
    private int getFleeingDirection()
    {
        List<int> potentialDirs = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            //Can't turn around, ignore
            if ((i + 2) % 4 == currentDir) continue;

            if (move.checkDirectionClear(num2vec(i)))
            {
                potentialDirs.Add(i);
            }
        }

        return potentialDirs[(int)Mathf.Floor(Random.value * potentialDirs.Count)];
    }

}
