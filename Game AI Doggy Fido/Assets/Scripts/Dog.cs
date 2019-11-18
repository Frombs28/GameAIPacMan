﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour {

    //Starting Sim at 7am
    public float
        hunger = 0.8f,
        energy = 0.8f,
        happiness = 0.4f,
        sleepiness = 0.1f,
        loyalty = 0.8f,
        loneliness = 0.2f,
        bathroom = 0.8f;

    public float
        foodInBowl = 0f;

    public bool ownerAtWork = false;


    void Update() {
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
        energy = 0.8f;
        happiness = 0.4f;
        sleepiness = 0.1f;
        loyalty = 0.8f;
        loneliness = 0.2f;
        bathroom = 0.8f;
        foodInBowl = 0;
    }
}
