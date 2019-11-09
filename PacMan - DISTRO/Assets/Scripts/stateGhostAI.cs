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
    Vector3 targetSquare;
    private Movement movement;
    private bool lastDirectionVert = false;
    public Vector3 posAtDecision;
    bool ableToMakeDecisions = true;
    public override void Enter()
    {
        base.Enter();
        targetSquare = blinkyAI.player.GetComponent<TargetTilesController>().blinkyTarget.transform.position;
        movement = blinkyAI.GetComponent<Movement>();
        movement._dir = Movement.Direction.left;

    }

    public override void Execute()
    {
        base.Execute();
        targetSquare = blinkyAI.player.GetComponent<TargetTilesController>().blinkyTarget.transform.position;
        updatePos();
        //check for new direction here


        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<moveState>();

        }


    }
    //when it reaches a dead, end, make the decision based on where the target direction is.
    public void updatePos() {
        if (!ableToMakeDecisions) {
            //Debug.Log("distance away from decision" + (posAtDecision - blinkyAI.transform.position).magnitude);

        }
        if ((posAtDecision - blinkyAI.transform.position).magnitude > 0.1f && !ableToMakeDecisions)
        {
            //Debug.Log(movement._dir);
            ableToMakeDecisions = true;
        }


        if (isAtIntersection() && ableToMakeDecisions)
        {
            //makes a decision
            //set the delta
            Vector3 directionToTarget = (targetSquare - blinkyAI.transform.position).normalized;
            movement._dir = setDirection(directionToTarget);
            lastDirectionVert = (movement._dir == Movement.Direction.up ||
                movement._dir == Movement.Direction.down);
            posAtDecision = blinkyAI.transform.position;

            ableToMakeDecisions = false;
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
    public Movement.Direction setDirection(Vector3 dirToTarget) {
        //check only left right

        List<KeyValuePair<float, Movement.Direction>> validOptions = new List<KeyValuePair<float, Movement.Direction>>();
        if (lastDirectionVert)
        {

            if (movement.checkDirectionClear(Vector2.left)) {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(Vector2.left, dirToTarget),
                    Movement.Direction.left));
            }
            if (movement.checkDirectionClear(Vector2.right))
            {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(Vector2.right, dirToTarget),
                    Movement.Direction.right));
            }
            if (movement.checkDirectionClear(movement.dirDict[movement._dir])) {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(movement.dirDict[movement._dir], dirToTarget),
                    movement._dir));
            }
                
        }
        else {
            if (movement.checkDirectionClear(Vector2.up))
            {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(Vector2.up, dirToTarget),
                    Movement.Direction.up));
            }
            if (movement.checkDirectionClear(Vector2.down))
            {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(Vector2.down, dirToTarget),
                    Movement.Direction.down));
            }
            if (movement.checkDirectionClear(movement.dirDict[movement._dir]))
            {
                validOptions.Add(new KeyValuePair<float, Movement.Direction>
                    (Vector2.Dot(movement.dirDict[movement._dir], dirToTarget),
                    movement._dir));
            }

        }

        //at a corner
        if (validOptions.Count == 1) {
            return validOptions[0].Value;
        }
        return getBestOption(validOptions);


    }
    public Movement.Direction getBestOption(List<KeyValuePair<float, Movement.Direction>> options) {
        KeyValuePair<float, Movement.Direction> bestOption = options[0];
        foreach (KeyValuePair<float, Movement.Direction> o in options) {
            //Debug.Log("checking dir " + o.Value);
            if (o.Key > bestOption.Key) {
                bestOption = o;
            }
        }
        //Debug.Log("choosing dir " + bestOption.Value);
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
