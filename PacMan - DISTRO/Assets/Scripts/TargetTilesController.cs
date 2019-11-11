using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTilesController : MonoBehaviour {

    public Transform blinkyTarget, pinkyTarget, inkyTarget, clydeTarget;

    public Transform blinky, clyde;

    public float cellSize = 1.5f;

    public Vector3 bottomLeft;

    public Vector3 bottomRight;

    public Vector3 topLeft;

    public Vector3 topRight;

    bool clydeSwitch = true;

    public int aiMode = 0;
    
    void Update() {
        // Switches between AI modes
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(aiMode == 0)
            {
                aiMode = 1;
            }
            else
            {
                aiMode = 0;
            }
        }
        
        // Original AI
        if (aiMode == 0)
        {
            print("Original");
            blinkyTarget.localPosition = new Vector3(0, 0, 0);
            pinkyTarget.localPosition = new Vector3(2 * cellSize, 0, 0);

            inkyTarget.position = pinkyTarget.position + (pinkyTarget.position - blinky.position);

            //If clyde within 8 tiles
            if (Vector3.Distance(transform.position, clyde.position) >= 8 * cellSize)
            {
                clydeTarget.position = blinkyTarget.position;
            }
            else
            {
                clydeTarget.position = bottomLeft;
            }
        }
        // Our AI
        else
        {
            print("Our AI");
            blinkyTarget.localPosition = new Vector3(0, 2 * cellSize, 0);
            pinkyTarget.localPosition = new Vector3(2 * cellSize, 0, 0);
            inkyTarget.localPosition = new Vector3(4 * cellSize, 0, 0);

            //If clyde within 8 tiles
            if (Vector3.Distance(transform.position, clyde.position) >= 8 * cellSize)
            {
                clydeTarget.position = transform.position;
                clydeSwitch = true;
            }
            else if(clydeSwitch)
            {
                clydeSwitch = false;
                int choice = Random.Range(0, 4);
                if(choice == 0)
                {
                    clydeTarget.position = bottomLeft;
                }
                else if(choice == 1)
                {
                    clydeTarget.position = bottomRight;
                }
                else if (choice == 2)
                {
                    clydeTarget.position = topLeft;
                }
                else
                {
                    clydeTarget.position = topRight;
                }
            }
        }
    }
}
