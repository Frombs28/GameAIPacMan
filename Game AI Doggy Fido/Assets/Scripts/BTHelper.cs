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
    public void tryToGoInside()
    {
        if (Dog.instance.outOfHouse)
        {
            GameManager.instance.PrintAction("fido wants to go inside");
            Task.current.Fail();
        }

        Task.current.Succeed();
    }

    [Task]
    public void tryToGoOutside() {
        if (!Dog.instance.outOfHouse)
        {
            GameManager.instance.PrintAction("fido wants to go outside");
            Task.current.Fail();
        }

        Task.current.Succeed();
    }
    [Task]
    public void botherOwner()
    {
        if(Dog.instance.foodInBowl <= 0.05f)
        {
            GameManager.instance.PrintAction("fido needs you to fill the bowl");
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
        GameManager.instance.PrintAction("Fido Has Eaten and Is Full!");
        Dog.instance.EatBowl();
        Task.current.Succeed();
    }

    [Task]
    public void checkIfHungry() {
        if (Dog.instance.hunger <= 0.8f)
        {
            Task.current.Succeed();
        }
        else {
            Task.current.Fail();
        }
    }
    [Task]
    public void bathroomOutside() {
        if (Dog.instance.outOfHouse) {
            //print dog goes to bathroom
            Dog.instance.bathroom = 0;
            Task.current.Succeed();
            return;
        }
        Task.current.Fail();
    }

    [Task]
    public void bathroomIntside()
    {

        if (Dog.instance.ownerAtWork)
        {
            //goes inside
            Task.current.Succeed();
        }
        else {
            Dog.instance.outOfHouse = true;
            Task.current.Succeed();
        }
    }

    [Task]
    public void checkIfHasToGoToBathroom() {
        if (Dog.instance.bathroom < 0.8f)
        {
            Task.current.Succeed();
        }
        else {
            Task.current.Fail();
        }
        
    }
    [Task]
    public void tapOnDoorToGoToBathRoom() {

        if (!Dog.instance.outOfHouse)
        {
            //Dog.instance.bathroom += 0.1f;
            if (Dog.instance.bathroom > 0.95f)
            {
                GameManager.instance.PrintAction("fido went to the bathroom inside");
                Dog.instance.bathroom = 0;
                Task.current.Succeed();

            }
            else {
                GameManager.instance.PrintAction("you need to let fido outside");
                Task.current.Fail();
            }
            
        }
        else {
            Task.current.Succeed();
            Dog.instance.bathroom = 0;
        }
    }

    [Task]
    public void asksForWalk() {
        if (Dog.instance.ownerAtWork)
        {
            Task.current.Fail();
        }
        else {
            if (Dog.instance.energy > 0.5f)
            {
                GameManager.instance.PrintAction("Fido wants you to take them for a walk");
                Task.current.Succeed();
            }
            else {
                Task.current.Fail();
            }
        }
    }
    [Task]
    public void asksForPet()
    {
        if (Dog.instance.ownerAtWork)
        {
            Task.current.Fail();
        }
        else {
            if (Dog.instance.loneliness > 0.5)
            {
                GameManager.instance.PrintAction("fido would like you to pet them");

                Task.current.Succeed();
            }
            else {
                Task.current.Fail();
            }
        }
    }
    [Task]
    public void doesNothing() {
        if (Dog.instance.ownerAtWork)
        {
            GameManager.instance.PrintAction("Fido misses you at work");
            Dog.instance.energy += 0.1f;
            Dog.instance.loneliness += 0.1f;
            Task.current.Succeed();
        }
        else
        {
            GameManager.instance.PrintAction("Fido is content but is getting more energetic");
            Dog.instance.energy += 0.1f;
            Task.current.Succeed();
        }
    }

    [Task]
    public void isFidoInside()
    {
        if (Dog.instance.outOfHouse)
        {
            GameManager.instance.PrintAction("you must let fido inside for him to go about his day");
            Task.current.Fail();
        }
        else
        {
            Task.current.Succeed();
        }
    }

    [Task]
    public void hasEnergyForStick()
    {
        if (Dog.instance.stickThrown)
        {
            if (Dog.instance.energy > 0.5f)
            {
                GameManager.instance.PrintAction("fido is fetching getting stick");
                Dog.instance.stickThrown = false;
                Task.current.Succeed();
            }
            else {
                GameManager.instance.PrintAction("fido does not have the energy to get the stick");
                Task.current.Succeed();
            }
        }
        else
        {
            Task.current.Fail();
        }
    }

    [Task]

    public void isGettingSleepy() {
        if (Dog.instance.sleepiness > 0.8f)
        {
            GameManager.instance.PrintAction("fido is too sleepy to do anything else in the day other than eat and bathroom");

            Task.current.Succeed();
        }
        else {
            Task.current.Fail();
        }
    }

    [Task]

    public void isSleeping() {

        
        if (Dog.instance.sleepiness > 0.95f)
        {
            if (Dog.instance.energy > 0.1)
            {
                GameManager.instance.PrintAction("fido heard a sound and got more energy");
                Dog.instance.energy -= 0.1f;
                Task.current.Succeed();
            }
            else {
                GameManager.instance.PrintAction("fido is going to sleep");

                Task.current.Fail();
            }
            
        }
        else {
            Task.current.Succeed();
        }
    }

    



}



