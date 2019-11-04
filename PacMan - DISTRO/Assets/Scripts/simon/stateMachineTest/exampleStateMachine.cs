using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exampleStateMachine : ByTheTale.StateMachine.MachineBehaviour
{

    public override void Start()
    {
        base.Start();
        Debug.Log("example state machine start");
    }
    private void Awake()
    {
        base.Initialize();
    }

    public override void AddStates()
    {

        Debug.Log("adding states");

        AddState<simonState1>();
        AddState<simonState2>();
        
        SetInitialState<simonState1>();

    }
}
