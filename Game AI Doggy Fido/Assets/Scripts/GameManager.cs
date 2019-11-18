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
    public Owner person;

    private static List<string> prevActions;

    public static int timeHours = 7, timeMinutes = 0;

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

        actionText.text = "------Recent Actions------";

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

    //F, Decrease Hunger, Slightly Increase Bond
    public void AddFoodToBowl() {
        if (printKeyPresses) {
            PrintAction("You Add Food To Fido's Bowl");
        }
        fido.hunger -= 1.0f;
        fido.loyalty += 0.05f;
    }

    //T, Decrease Hunger Slightly, Increase Bond, Increase Happiness
    public void GiveDogTreat() {
        if (printKeyPresses) {
            PrintAction("You Give Fido A Treat");
        }
    }

    //K
    public void ThrowStickForFetch() {
        if (printKeyPresses) {
            PrintAction("You Throw A Stick For Fetch");
        }
    }
    
    //P
    public void Pet() {
        if (printKeyPresses) {
            PrintAction("You Pet Fido");
        }
    }

    //B
    public void BellyRub() {
        if (printKeyPresses) {
            PrintAction("You Give Fido A Belly Rub");
        }
    }

    //W
    public void GoOnWalk() {
        if (printKeyPresses) {
            PrintAction("You Go On A Walk With Fido");
        }
    }

    //L
    public void LeaveDogAlone() {
        if (printKeyPresses) {
            PrintAction("You Leave Fido Alone");
        }
    }

    //G
    public void GoToWork() {
        if (person.atWork) return;
        if (printKeyPresses) {
            PrintAction("You Go To Work");
        }
        person.atWork = true;
    }

    //A
    public void GetHomeFromWork() {
        if (!person.atWork) return;
        if (printKeyPresses) {
            PrintAction("You Come Home From Work");
        }
        person.atWork = false;
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
    }

    //D
    public void NewDay() {
        if (printKeyPresses) {
            PrintAction("A New Day Starts");
        }
        SetTime(7, 0);
    }

    //S
    public void MysterySound() {
        if (printKeyPresses) {
            PrintAction("A Mysterious Sound Is Heard");
        }
    }

    //R
    public void RestartSimulation() {
        SetTime(7, 0);
        prevActions.Clear();
        if (printKeyPresses) {
            PrintAction("Restarting Sim...");
        }
        fido.Reset();
        person.Reset();
    }
}
