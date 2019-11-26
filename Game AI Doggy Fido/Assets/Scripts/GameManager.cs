using System.Collections;
using System.Collections.Generic;
using Panda;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text actionText;
    public int prevActionsToTrack = 12;
    public static int _prevActionsToTrack;
    public Text timeText;

    public Dog fido;

    private static List<string> prevActions;

    public static int timeHours = 7, timeMinutes = 0;

    public int timeSinceBellyRubMinutes = 100;

    //For Debug
    public bool printKeyPresses = true;

    public PandaBehaviour pd;

    public bool newDay = false;

    public static GameManager instance;
    void Start() {
        _prevActionsToTrack = prevActionsToTrack;
        prevActions = new List<string>();
        instance = this;
        pd.Tick();
    }

    int t = 0;

    void Update() {
        //Make time correct
        while (timeMinutes >= 60) {
            timeMinutes -= 60;
            timeHours++;
            timeHours %= 24;
            
        }

        //Check for buttons presses
        if (Input.GetKeyDown(KeyCode.F)) {
            AddFoodToBowl();
            


        }
        if (Input.GetKeyDown(KeyCode.T)) {
            GiveDogTreat();
            
        }
        if (Input.GetKeyDown(KeyCode.K)) {
            ThrowStickForFetch();
            
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            Pet();
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            BellyRub();
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            GoOnWalk();
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            LeaveDogAlone();
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            GoToWork();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            GetHomeFromWork();
        }
        if (Input.GetKeyDown(KeyCode.I)) {
            Idle();
        }
        if (Input.GetKeyDown(KeyCode.H)) {
            Hour();
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            NewDay();
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            MysterySound();
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartSimulation();
        }

        //Update Display
        UpdateText();
    }

    private void UpdateText() {
        //Update Time
        timeText.text = "Time:\n" + timeHours.ToString("D2") + ":" + timeMinutes.ToString("D2");

        actionText.text = "==========Recent Actions==========";

        foreach (string action in prevActions) {
            actionText.text += "\n" + action;
        }
    }

    string lastText = "";
    public void PrintAction(string text) {
        if (lastText.Equals(text)) {
            return;
        }
        while (prevActions.Count > _prevActionsToTrack-1) {
            prevActions.RemoveAt(0);
        }

        prevActions.Add(text);
        lastText = text;
    }

    public void AddTime(int minutes) {

        timeMinutes += minutes;
        Dog.instance.bathroom += minutes / 300f;
        Dog.instance.hunger += minutes / 300f;
        if (timeHours >= 17 || timeHours < 7) {
            Dog.instance.sleepiness += 0.1f;
            Dog.instance.energy -= 0.1f;
        }
        if(!newDay && timeHours >= 7 && timeHours < 8)
        {
            Dog.instance.WakeUp();
            PrintAction("Fido wakes up!");
            newDay = true;
        }
        if(newDay && timeHours >= 8)
        {
            newDay = false;
        }
        pd.Tick();


    }

    public static void SetTime(int hours, int minutes) {
        timeHours = hours;
        timeMinutes = minutes;
    }




    //Actions///////////////////////////////

    //F, Decrease Hunger, Slightly Increase Bond
    public void AddFoodToBowl() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Give Food While At Work");
            }
            return;
        }

        if (fido.foodInBowl > 0.95) {
            if (printKeyPresses) {
                PrintAction("There is already food in Fido's Bowl");
            }
        } else {
            if (printKeyPresses) {
                PrintAction("You Add Food To Fido's Bowl");
            }
            fido.foodInBowl = 1.0f;
            AddTime(5);
        }
    }

    //T, Decrease Hunger Slightly, Increase Bond, Increase Happiness
    public void GiveDogTreat() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Give A Treat While At Work");
            }
            return;
        }

        if (printKeyPresses) {
            PrintAction("You Give Fido A Treat");
            AddTime(4);
        }
        fido.hunger -= 0.04f;
        fido.energy += 0.05f;
        fido.happiness += 0.05f;
        fido.loyalty += 0.01f;
    }

    //K
    public void ThrowStickForFetch() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Throw A Stick While At Work");
            }
            return;
        }

        if (fido.stickThrown) {
            if (printKeyPresses) {
                PrintAction("Fido Still Hasn't Gotten The Stick From Last Time");
            }
        } else {
            if (printKeyPresses) {
                PrintAction("You Throw A Stick For Fetch");
            }
            fido.stickThrown = true;
        }
        AddTime(10);
    }
    
    //P
    public void Pet() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Pet Fido While At Work");
            }
            return;
        }

        if (fido.sleeping) {
            if (printKeyPresses) {
                PrintAction("He's Sleeping, Leave Him Alone :(");
            }
            return;
        }

        if (fido.loneliness < 0.1f)
        {
            if (printKeyPresses)
            {
                PrintAction("Fido does not need any more petting");
            }
        }

        if (printKeyPresses) {
            PrintAction("You Pet Fido");
            AddTime(4);
        }

        fido.happiness += 0.05f;
        fido.loyalty += 0.02f;
        fido.loneliness -= 0.05f;
    }

    //B
    public void BellyRub() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Belly Rub Fido While At Work");
            }
            return;
        }
        if (fido.sleeping) {
            if (printKeyPresses) {
                PrintAction("He's Sleeping, Leave Him Alone :(");
            }
            return;
        }

        if (timeSinceBellyRubMinutes < 30) {
            if (printKeyPresses) {
                PrintAction("You Give Fido A Belly Rub, He Enjoys It");
                fido.loyalty += 0.05f;
                fido.happiness += 0.08f;
                timeSinceBellyRubMinutes = 0;
                AddTime(10);
            }
        } else {
            if (printKeyPresses) {
                PrintAction("You Already Recently Gave Fido A Belly Rub, He Dislikes It");
                fido.loyalty -= 0.04f;
                fido.happiness -= 0.1f;
                timeSinceBellyRubMinutes = 0;
            }
        }

    }

    //W
    public void GoOnWalk() {

        if (Dog.instance.outOfHouse) {
            PrintAction("Letting Fido back inside");
            Dog.instance.outOfHouse = false;
            return;
        }
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Go On A Walk While At Work");
            }
            return;
        }

        if (fido.sleeping) {
            if (printKeyPresses) {
                PrintAction("Fido Is Sleeping, He Doesn't Want To Walk");
            }
            return;
        }

        if (fido.energy < 0.4f && fido.bathroom < 0.8f) {
            if (printKeyPresses) {
                PrintAction("Fido Is Tired, He Doesn't Want To Walk");
                return;
            }
        }

        if (printKeyPresses) {
            PrintAction("You Go On A Walk With Fido");
        }

        if (printKeyPresses && fido.bathroom >= 0.3f)
        {
            PrintAction("Fido goes to the bathroom outside :)");
            fido.bathroom = 0f;
        }

        fido.energy -= 0.2f;
        fido.happiness += 0.1f;
        fido.loyalty += 0.02f;
        Dog.instance.outOfHouse = true;
        AddTime(20);
    }

    //L
    public void LeaveDogAlone() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Can't Leave Fido Alone While At Work");
            }
            return;
        }

        if (fido.sleeping) {
            if (printKeyPresses) {
                PrintAction("Fido is already asleep");
            }
            return;
        }

        if (printKeyPresses) {
            PrintAction("You Leave Fido Alone");
            AddTime(10);
        }
        fido.FallAsleep();
    }

    //G
    public void GoToWork() {
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You're Already At Work");
            }
            return;
        }
        if (Dog.instance.outOfHouse) {
            PrintAction("fido will run away if you do not let them inside");
            return;
        }

        if (printKeyPresses) {
            PrintAction("You Go To Work");
            AddTime(10);
        }
        fido.ownerAtWork = true;
    }

    //A
    public void GetHomeFromWork() {
        if (!fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You Are Already Home");
            }
            return;
        }

        if (printKeyPresses) {
            PrintAction("You Come Home From Work");
            AddTime(0);
        }
        fido.ownerAtWork = false;
    }

    //I
    public void Idle() {

        AddTime(15);

    }

    //H
    public void Hour() {

        AddTime(60);

        if (fido.ownerAtWork) {

        } else {

        }
    }

    //D
    public void NewDay() {
        //Need to have certain conditions met
        if (!fido.sleeping) {
            if (printKeyPresses) {
                PrintAction("Fido Isn't Sleeping");
            }
            return;
        }
        if (fido.ownerAtWork) {
            if (printKeyPresses) {
                PrintAction("You can't start a new day while at work");
            }
            return;
        }


        if (printKeyPresses) {
            PrintAction("A New Day Starts");
        }
        SetTime(7, 0);

        fido.WakeUp();
    }

    //S
    public void MysterySound() {
        if (printKeyPresses) {
            PrintAction("A Mysterious Sound Is Heard");
        }

        //Different things based on different float levels
        AddTime(0);
        fido.SoundHeard();
    }

    //R
    public void RestartSimulation() {
        SetTime(7, 0);
        prevActions.Clear();
        if (printKeyPresses) {
            PrintAction("Restarting Sim...");
        }
        fido.Reset();
    }
}
