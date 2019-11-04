using UnityEngine;
using System.Collections;

public class simonState2 : ByTheTale.StateMachine.State
{
    public exampleStateMachine exampleCharacter { get { return (exampleStateMachine)machine; } }
    // Use this for initialization
    
    public override void Enter()
    {
        base.Enter();
        Debug.Log("simon state 2 entered");
    }

    public override void Execute()
    {
        base.Execute();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            machine.ChangeState<simonState1>();
        }
    }

    public override void PhysicsExecute()
    {
        base.PhysicsExecute();

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("state 2 exit");
    }
}
