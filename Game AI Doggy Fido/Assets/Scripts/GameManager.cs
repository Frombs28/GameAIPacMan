using System.Collections;
using System.Collections.Generic;
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

    void Start() {
        _prevActionsToTrack = prevActionsToTrack;
        prevActions = new List<string>();
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

    public static void PrintAction(string text) {
        while (prevActions.Count > _prevActionsToTrack-1) {
            prevActions.RemoveAt(0);
        }

        prevActions.Add(text);
    }

    public static void AddTime(int minutes) {
        timeMinutes += minutes;
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

        if(fido.hunger >= 0.80f && printKeyPresses)
        {
            PrintAction("Fido is Full");
            return;
        }

        if (printKeyPresses) {
            PrintAction("You Give Fido A Treat");
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

        if (printKeyPresses) {
            PrintAction("You Pet Fido");
        }

        fido.happiness += 0.05f;
        fido.loyalty += 0.02f;
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

        if (fido.energy < 0.4f) {
            if (printKeyPresses) {
                PrintAction("Fido Is Tired, He Doesn't Want To Walk");
            }
        }

        if (!fido.outOfHouse && printKeyPresses) {
            PrintAction("You Go On A Walk With Fido");
            fido.outOfHouse = true;
            fido.energy -= 0.4f;
            fido.happiness += 0.1f;
            fido.loyalty += 0.02f;
            fido.bathroom = 0f;
        }

        if (fido.outOfHouse && printKeyPresses)
        {
            PrintAction("You Bring Fido Back Inside");
            fido.outOfHouse = false;
        }
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

        if (printKeyPresses) {
            PrintAction("You Go To Work");
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
        }
        fido.ownerAtWork = false;
    }

    //I
    public void Idle() {
        if (printKeyPresses) {
            PrintAction("15 Minutes Pass");
        }
        AddTime(15);
    }

    //H
    public void Hour() {
        if (printKeyPresses) {
            PrintAction("1 Hour Passes");
        }
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

    public void SendAction(string message)
    {
        PrintAction(message);
    }
}
