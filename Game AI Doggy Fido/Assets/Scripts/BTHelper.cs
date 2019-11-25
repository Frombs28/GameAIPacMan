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
    public void testIdle() {

        Debug.Log("idle");
        Task.current.Succeed();
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
    public void goInside()
    {
        if (Dog.instance.outOfHouse)
        {
            GameManager.instance.PrintAction("fido wants to go inside to eat");
            Task.current.Fail();
        }

        Task.current.Succeed();
    }

    [Task]
    public void botherOwner()
    {
        if(Dog.instance.foodInBowl <= 0.05f)
        {
            Task.current.Fail();
        }
        else
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void eatFood()
    {
        // Print ("Fido Has Eaten and Is Full!");
        Dog.instance.EatBowl();
        Task.current.Succeed();
    }
}
