using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the central lights in each level and controls their interactions with moveable objects.
public class LightManager : MonoBehaviour
{
    [SerializeField] private int lightTarget;
    [SerializeField] private GameObject[] targets;
    [SerializeField] private GameObject divider;
    [SerializeField] private AudioClip sound;
    private int litUp;

    //Called from Light Trigger when an unlit light has been hit.
    public void LightUp() {
        litUp++;
        if (litUp == lightTarget) {
            divider.GetComponent<Animator>().SetTrigger("Move");
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + (lightTarget * 100)); // adds a bonus when all are activated.
        }
    }

    //I would have used PlayOneShot for this, but that does not support sounds of different pitch.
    //So instead, a new audio source is created (and destroyed) each time the ball hits the trigger.
    private void PlaySound() {
        AudioSource player = gameObject.AddComponent<AudioSource>();
        player.clip = sound;
        if (litUp == lightTarget ) {
            player.pitch = 1.2f;
        } else {
            switch (litUp) {
                case 1:
                    player.pitch = 0.4f; 
                    break;
                case 2:
                    player.pitch = 0.6f; 
                    break;
                case 3:
                    player.pitch = 0.8f; 
                    break;
                case 4:
                    player.pitch = 1; 
                    break;
            }
        }
        player.volume = PlayerPrefs.GetFloat("volume", 0.5f) * 0.9f;
        player.Play();
        Destroy(player, sound.length / player.pitch);
    }

    //Called from Light Trigger whenever any light is hit.
    public void MoveTargets() {
        PlaySound();
        for (int i = 0; i < targets.Length; i++) {
            targets[i].GetComponent<Animator>().SetTrigger("Move");
        }
    }
}
