using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the scattered lights throughout the levels.
public class LightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject lightUp; // the corresponding middle light
    private bool pressed;
    private bool active = true;

    //When the ball enters its trigger, it lights up and adds score (provided it is not on cooldown)
    void OnTriggerEnter(Collider other) {
        if (!other.CompareTag("Ball")) {
            return;
        }
        if (!pressed) {
            pressed = true;
            lightUp.GetComponentInParent<LightManager>().LightUp();
        }
        if (active) {
            lightUp.GetComponent<Animator>().SetTrigger("Flash");
            GetComponent<Animator>().SetTrigger("Flash");
            PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score") + 10);
            lightUp.GetComponentInParent<LightManager>().MoveTargets();
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer() {
        active = false;
        yield return new WaitForSeconds(1);
        active = true;
    }
}
