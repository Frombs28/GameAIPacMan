using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTilesController : MonoBehaviour {

    public Transform blinkyTarget, pinkyTarget, inkyTarget, clydeTarget;

    public Transform blinky, clyde;

    public float cellSize = 1.5f;

    public Vector3 bottomLeft;
    
    void Update() {
        blinkyTarget.localPosition = new Vector3(0, 0, 0);
        pinkyTarget.localPosition = new Vector3(2 * cellSize, 0, 0);

        inkyTarget.position = pinkyTarget.position + (pinkyTarget.position - blinky.position);

        //If clyde within 8 tiles
        if (Vector3.Distance(transform.position, clyde.position) <= 8*cellSize) {
            clydeTarget.position = blinkyTarget.position;
        } else {
            clydeTarget.position = bottomLeft;
        }
    }
}
