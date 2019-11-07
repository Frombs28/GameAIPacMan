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
    private Movement.Direction lastDirection;
    

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
        
        
        //int randStart = Random.Range(0, 10);
        //for (int i = 0; i < movement.dirArray.Length; i++)
        //{
        //    if (movement.checkDirectionClear(movement.dir))
        //    {
                
        //        if ((int)movement._dir == i)
        //        {
        //            Debug.Log("skipping " + (Movement.Direction)i);
        //            continue;
        //        }

        //        if (movement.dirArray[i] * -1 == movement.dirArray[(int)movement._dir])
        //        {
        //            Debug.Log("skipping " + (Movement.Direction)i);
        //            continue;
        //        }
        //        Debug.Log((Movement.Direction)i);
        //        movement._dir = (Movement.Direction)i;
                

        //        break;
        //    }
            


        //    //int index = (i + randStart) % movement.dirArray.Length;

            
            
        //}


        
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
