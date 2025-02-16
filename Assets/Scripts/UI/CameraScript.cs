using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the in-game camera and controls its position.
//When the level is changed, it is called from LevelEndTrigger and the position is updated.
public class CameraScript : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;

    private void Start() {
        ChangeLevel();
    }

    public void ChangeLevel() {
        int level = LevelEndTrigger.thisLevel;
        transform.position = positions[level];
    }
}
