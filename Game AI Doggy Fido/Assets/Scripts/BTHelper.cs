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

    [Task]
    public void test1(){
        if (Input.GetKeyDown(KeyCode.Q)) {
            Task.current.Succeed();
        }
    }
    [Task]
    public void test2()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Task.current.Succeed();
        }
    }
    [Task]
    public void test3()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Task.current.Succeed();
        }
    }


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

    [Task]
    public void walkOverToBowl()
    {
        if (Dog.instance.energy > 0)
        {
            Task.current.Succeed();
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]
    public void eatFood()
    {

        if (Dog.instance.hunger > 1) {
            Task.current.Fail();
        }
        Dog.instance.energy += 0.1f;
        Dog.instance.foodInBowl -= 0.1f;

        Task.current.Succeed();


    }




}
