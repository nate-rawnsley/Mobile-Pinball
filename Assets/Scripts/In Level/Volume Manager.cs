using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on every object that has an audio source.
//It adjusts the volume based on the currently set volume in the options (using playerprefs).
//Each script has a customised multiplier, as the audio clips are not the same volume to begin.
public class VolumeManager : MonoBehaviour
{
    [SerializeField] private float multiplier;

    private void Update() {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("volume", 0.5f) * multiplier;
    }
}
