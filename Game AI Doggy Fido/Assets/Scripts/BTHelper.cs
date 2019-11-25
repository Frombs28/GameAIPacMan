using UnityEngine;
using System.Collections;
using Panda;

public class BTHelper : MonoBehaviour
{

    [Task]
    bool isRunning = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //inside these there should be conditional statements based on the data in Gamemanager
    [Task]
    public void testIdle() {
        
        Debug.Log("idle");
        Task.current.Succeed();
    }
    [Task]
    public void test2()
    {
        Debug.Log("idle2");
        if (Input.GetKeyDown(KeyCode.A)) {
            Dog.instance.sleeping = true;
        }
        if (Dog.instance.sleeping) {
            Task.current.Succeed();
        }
    }

    [Task]
    public void doIHaveFood()
    {
        if (Dog.instance.foodInBowl > 0)
        {
            Task.current.Succeed();
        }
        else {
            Task.current.Fail();
        }
    }
       
}
