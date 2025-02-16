using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on triggers that border each level.
//They act as both a respawn trigger and a trigger to move the camera up/down a level.
public class LevelEndTrigger : MonoBehaviour
{
    private int startLevel;
    [SerializeField] private int triggerNum;
    private bool entered;
    [SerializeField] private CameraScript mainCamera;
    private bool ended;

    public static int thisLevel;

    private void Awake() {
        startLevel = PlayerPrefs.GetInt("curLevel", 0);
        thisLevel = startLevel;
    }

    //When a player exits a trigger, it checks the number of the trigger against the start level.
    //If it is the start level, the player loses a life - otherwise, the level changes.
    //(Exit is used instead of enter to minimise times where the ball enters from below but falls back down)
    private void OnTriggerExit(Collider other) {
        if (ended) {
            return;
        }
        if (other.CompareTag("Ball")) {
            if (triggerNum > startLevel) {
                StartCoroutine(PhaseThrough(other.gameObject));
                if (PlayerPrefs.GetInt("unlockedLevel", 1) < triggerNum) {
                    PlayerPrefs.SetInt("unlockedLevel", triggerNum);
                }
            } else {
                ended = other.gameObject.GetComponent<Ball>().Death(startLevel);
            }
        }
    }

    //The ball's layer is changed so that it can phase through the bottom of the next level, before changing back.
    //Explosion force is used to make the transition even more smooth.
    private IEnumerator PhaseThrough(GameObject ball) {
        int phaseThroughLayer = LayerMask.NameToLayer("Default");
        int defaultLayer = LayerMask.NameToLayer("Ball");
        ball.layer = phaseThroughLayer;
        if (entered) { // (if the player has entered it once and enters again, reduce level by one)
            thisLevel = triggerNum - 1;
            mainCamera.ChangeLevel();
        } else {
            //If the player enters the trigger to move to the next level, but doesn't exit at the top, cancel the process.
            if (ball.transform.position.y < transform.position.y) {
                ball.layer = defaultLayer;
                yield break;
            }
            thisLevel = triggerNum;
            mainCamera.ChangeLevel();
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            Vector3 explodePos = ball.transform.position;
            explodePos.y -= 1;
            rb.AddExplosionForce(1000f, explodePos, 0, 75f);
        }
        yield return new WaitForSeconds(0.5f);
        entered = !entered;
        ball.layer = defaultLayer;
    }
}
