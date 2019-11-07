using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stateGhostAI : ByTheTale.StateMachine.MachineBehaviour
{
    // Start is called before the first frame update
    Movement movementScript;
    public GameObject player;

    public override void AddStates()
    {
        AddState<chaseState>();

        SetInitialState<chaseState>();
    }

    private void Start()
    {
        base.Start();
        player = GameObject.Find("PacMan(Clone)");

        movementScript = gameObject.AddComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}


public class chaseState : ByTheTale.StateMachine.State
{

    public stateGhostAI blinkyAI { get { return (stateGhostAI)machine; } }
    public GameObject targetSquare;
    private Movement movement;
    private bool lastDirectionVert = false;

    public override void Enter()
    {
        base.Enter();
        targetSquare = blinkyAI.player;
        movement = blinkyAI.GetComponent<Movement>();
        movement._dir = Movement.Direction.left;

    }

    public override void Execute()
    {
        base.Execute();
        targetSquare = blinkyAI.player;
        setLastDirectionVert();
        updatePos();
        setLastDirectionVert();
        //check for new direction here


        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<moveState>();

        }


    }
    //when it reaches a dead, end, make the decision based on where the target direction is.
    public void updatePos() {

        if (movement._dir == Movement.Direction.still) {
            Debug.Log(lastDirectionVert);
            Vector3 directionToTarget = targetSquare.transform.position - blinkyAI.transform.position;


            if (lastDirectionVert)
            {
                //right is a valid option
                if (movement.checkDirectionClear(Vector2.right))
                {
                    //left is a valid options
                    if (movement.checkDirectionClear(Vector2.left))
                    {
                        //check the best one
                        movement._dir = getClosestDirection(directionToTarget);
                    }
                    else
                    {
                        //only right is valid
                        movement._dir = Movement.Direction.right;
                    }

                }
                else {
                    movement._dir = Movement.Direction.left;
                }

            }
            else {
                if (movement.checkDirectionClear(Vector2.up))
                {
                    if (movement.checkDirectionClear(Vector2.down))
                    {
                        movement._dir = getClosestDirection(directionToTarget);
                    }
                    else
                    {
                        movement._dir = Movement.Direction.up;
                    }
                }
                else {
                    movement._dir = Movement.Direction.down;
                }
            }
            lastDirectionVert = !lastDirectionVert;
        }

        if (isAtIntersection()) {
            
        }

        //if (isAtIntersection()) {
        //    //check if taking the intersection would be better than staying on your current trajectory

        //    //gets the angle between the ghosts direction and the position of pacman
        //    float currentDirAngle = Vector2.Angle(movement.dirArray[(int)movement._dir], targetSquare.transform.position);

        //    if (lastDirectionVert) {
        //        //check for left and right
        //        if()

        //    }
        //}

        

        
    }

    public bool isAtIntersection() {
        if (lastDirectionVert)
        {
            if (movement.checkDirectionClear(Vector2.left) || movement.checkDirectionClear(Vector2.right))
            {
                return true;
            }
        }
        else {
            if (movement.checkDirectionClear(Vector2.up) || movement.checkDirectionClear(Vector2.down))
            {
                return true;
            }
        }

        return false;
    }

    public bool setLastDirectionVert() {
        if (movement._dir == Movement.Direction.down || movement._dir == Movement.Direction.up) {
            return true;
        }
        return false;
    }


    //gets the closest direction
    public Movement.Direction getClosestDirection(Vector3 dirToTarget) {
        //check only left right
        if (lastDirectionVert)
        {
            //check if better option than current direction

            float leftA = Vector2.Angle(Vector3.left, dirToTarget);
            float rightA = Vector2.Angle(Vector3.right, dirToTarget);

            if (movement._dir == Movement.Direction.still) {
                return getBestOption(new List<KeyValuePair<float, Movement.Direction>>() {
                    new KeyValuePair<float, Movement.Direction>(leftA, Movement.Direction.left),
                    new KeyValuePair<float, Movement.Direction>(rightA, Movement.Direction.right)
                });
            }

            float currentAngle = Vector2.Angle(movement.dirArray[(int)movement._dir], dirToTarget);

            return getBestOption(new List<KeyValuePair<float, Movement.Direction>>() {
                    new KeyValuePair<float, Movement.Direction>(currentAngle, movement._dir),
                    new KeyValuePair<float, Movement.Direction>(leftA, Movement.Direction.left),
                    new KeyValuePair<float, Movement.Direction>(rightA, Movement.Direction.right)
                });

        }
        else {
            float upA = Vector2.Angle(Vector3.up, dirToTarget);
            float downA = Vector2.Angle(Vector3.down, dirToTarget);

            if (movement._dir == Movement.Direction.still)
            {
                return getBestOption(new List<KeyValuePair<float, Movement.Direction>>() {
                    new KeyValuePair<float, Movement.Direction>(upA, Movement.Direction.up),
                    new KeyValuePair<float, Movement.Direction>(downA, Movement.Direction.down)
                });
            }

            float currentAngle = Vector2.Angle(movement.dirArray[(int)movement._dir], dirToTarget);

            return getBestOption(new List<KeyValuePair<float, Movement.Direction>>() {
                    new KeyValuePair<float, Movement.Direction>(currentAngle, movement._dir),
                    new KeyValuePair<float, Movement.Direction>(upA, Movement.Direction.up),
                    new KeyValuePair<float, Movement.Direction>(downA, Movement.Direction.down)
                });
        }

        
    }
    public Movement.Direction getBestOption(List<KeyValuePair<float, Movement.Direction>> options) {
        KeyValuePair<float, Movement.Direction> bestOption = options[0];
        foreach (KeyValuePair<float, Movement.Direction> o in options) {
            Debug.Log("cheking: " + o.Key + " with angle to player: " + o.Value);
            if (o.Key < bestOption.Key) {
                bestOption = o;
            }
        }

        return bestOption.Value;
    }
}

public class scatterState : ByTheTale.StateMachine.State
{

    public stateGhostAI blinkyAI { get { return (stateGhostAI)machine; } }
    

    public override void Enter()
    {
        base.Enter();


    }

    public override void Execute()
    {
        base.Execute();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<moveState>();

        }


    }
}

public class frightenedState : ByTheTale.StateMachine.State
{

    public stateGhostAI blinkyAI { get { return (stateGhostAI)machine; } }

    public override void Enter()
    {
        base.Enter();


    }

    public override void Execute()
    {
        base.Execute();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<moveState>();

        }


    }
}
