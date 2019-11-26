using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{

    //Starting Sim at 7am
    public float
        hunger = 0.8f,
        energy = 0.8f,
        happiness = 0.4f,
        sleepiness = 0.1f,
        loyalty = 0.8f,
        loneliness = 0.2f,
        bathroom = 0.8f,
        foodInBowl = 0f;

    public bool 
        ownerAtWork = false,
        stickThrown = false,
        sleeping = false,
        outOfHouse = false,
        soundHeard = false;

    public GameManager gm;

    //State Transitions Goes Here?
    //To Print To Screen: GameManager.PrintAction("");



    public void FallAsleep() {
        //Fido is left alone and it calls this. It doesn't necessarily trigger sleep
    }
    public void SoundHeard() {
        energy += 0.5f;
        soundHeard = true;
        //Print different things based on what the float values above are
    }
    public static Dog instance;
    public void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void GoInside()
    {
        GameManager.instance.PrintAction("Fido Wants To Go Back Inside");
    }

    public void GoOutside()
    {
        GameManager.instance.PrintAction("Fido Wants To Go Outside!");
    }

    public void EatBowl()
    {
        hunger -= foodInBowl;
        foodInBowl = 0;
    }

    public void Update() {
        //Don't Let Values Exceed 1 or go below 0
        happiness = Mathf.Clamp(happiness, 0, 1);
        sleepiness = Mathf.Clamp(sleepiness, 0, 1);
        energy = Mathf.Clamp(energy, 0, 1);
        hunger = Mathf.Clamp(hunger, 0, 1);
        loyalty = Mathf.Clamp(loyalty, 0, 1);
        loneliness = Mathf.Clamp(loneliness, 0, 1);
        bathroom = Mathf.Clamp(bathroom, 0, 1);
        foodInBowl = Mathf.Clamp(foodInBowl, 0, 1);
    }

    public void Reset() {
        hunger = 0.8f;
        energy = 1.0f;
        happiness = 0.4f;
        sleepiness = 0f;
        loyalty = 0.8f;
        loneliness = 0.2f;
        bathroom = 0.8f;
        foodInBowl = 0;
        ownerAtWork = false;
        stickThrown = false;
        outOfHouse = false;
        soundHeard = false;
        sleeping = false;
    }

    //Partial Reset
    public void WakeUp() {
        sleeping = false;
        hunger = 0.8f;
        energy = 1.0f;
        sleepiness = 0f;
        bathroom = 0.8f;
        outOfHouse = false;
        soundHeard = false;
        stickThrown = false;
        ownerAtWork = false;
    }

    
}
