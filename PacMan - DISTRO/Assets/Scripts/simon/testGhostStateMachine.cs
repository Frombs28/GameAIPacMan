using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testGhostStateMachine : ByTheTale.StateMachine.MachineBehaviour
{
    // Start is called before the first frame update
    Movement movementScript;

    private void Start()
    {
        base.Start();

        movementScript = gameObject.AddComponent<Movement>();
    }

    public override void AddStates()
    {
        AddState<moveState>();
        AddState<stopState>();
        SetInitialState<stopState>();
    }
}

public class moveState : ByTheTale.StateMachine.State {

    public testGhostStateMachine exampleCharacter { get { return (testGhostStateMachine)machine; } }
    private Movement movement;

    

    public override void Enter()
    {
        
        base.Enter();

        movement = exampleCharacter.GetComponent<Movement>();
        movement._dir = Movement.Direction.left;
        Debug.Log("movement state entered");

       
    }

    public override void Execute()
    {
        base.Execute();
        
        if (movement._dir == Movement.Direction.still)
        {
            Debug.Log("stuck");

            int randStart = Random.Range(0, 10);
            for (int i = 0; i < movement.dirArray.Length; i++)
            {
                if ((int)movement._dir == i)
                {
                    Debug.Log("skipping " + (Movement.Direction)i);
                    continue;
                }


                int index = (i + randStart) % movement.dirArray.Length;

                if (movement.dirArray[index] * -1 == movement.dirArray[(int)movement._dir])
                {
                    continue;
                }
                if (movement.checkDirectionClear(movement.dirArray[index]))
                {
                    Debug.Log((Movement.Direction)index);
                    movement._dir = (Movement.Direction)index;
                    break;
                }
            }


        }
    }
}

public class stopState : ByTheTale.StateMachine.State
{

    public testGhostStateMachine exampleCharacter { get { return (testGhostStateMachine)machine; } }

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
