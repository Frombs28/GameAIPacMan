using UnityEngine;
using System.Collections;

public class simonState1 : ByTheTale.StateMachine.State
{
    // Use this for initialization
    public exampleStateMachine exampleCharacter { get { return (exampleStateMachine)machine; } }


    public override void Enter()
    {
        base.Enter();
        Debug.Log("simon state 1 entered");
    }

    public override void Execute()
    {

        base.Execute();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<simonState2>();
        }
    }

    public override void PhysicsExecute()
    {
        base.PhysicsExecute();

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("state 1 exit");
    }
}
