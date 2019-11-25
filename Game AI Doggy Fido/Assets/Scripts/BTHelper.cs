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
    public void wantToGoInside()
    {
        if (Dog.instance.outOfHouse)
        {
            Dog.instance.GoInside();
        }

        Task.current.Succeed();
    }

    [Task]
    public bool NotInside()
    {
        if (Dog.instance.outOfHouse)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    [Task]
    public void walkToDish()
    {
        Dog.instance.
    }

    [Task]
    public void botherOwner()
    {
        if(Dog.instance.foodInBowl <= 0.05f)
        {
            Dog.instance.gm.SendAction("Fido Is Begging You To Fill The Food Bowl!");
        }
        else
        {

        }
    }

}
