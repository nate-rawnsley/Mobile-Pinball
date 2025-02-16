using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script is on the 'launchers' in the bottom right of every level.
//This uses a spring joint, allowing the player to build and release tension to launch.
public class Launcher : MonoBehaviour
{
    private float startPos;
    [SerializeField] private float endPos;
    private float currentDistance;

    private float prevTime = 0;
    private bool lerping;
    private bool released;
    private bool played = true;

    [SerializeField] private Ball ball;
    [SerializeField] private int number;

    [SerializeField] private AudioClip tense;
    [SerializeField] private AudioClip release;

    private float tenseNum = 0.1f;

    private void Start() {
        startPos = transform.position.y;
        currentDistance = 0;
    }

    //Called from an event in TouchManager.
    //While the player is not holding an input, the values are reset, allowing it to spring upwards.
    public void OnRelease() {
        tenseNum = 0.1f;
        currentDistance = 0;
        prevTime = 0;
        if (!lerping) {
            GetComponent<Rigidbody>().isKinematic = false;
            if (!played) {
                GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().time = 1;
                played = true;
            }
        }
        released = true;
    }

    //If the player is holding and input and the launcher is active, this runs.
    //The value of 'currentDistance' depends on how far the player moved in their input.
    //It also does not exceed 0 or subceed the (negative) distance between the start and end points.
    public void OnHold(float delta) {
        if (ball.launcherActive && LevelEndTrigger.thisLevel == number) {
            GetComponent<Rigidbody>().isKinematic = true;
            float tempDist = currentDistance + delta * 2 * Time.deltaTime;
            currentDistance = tempDist > 0 ? 0 : tempDist < endPos - startPos ? endPos - startPos : tempDist;
            StartCoroutine(MoveSpring());
        }
    }

    //A lerp algorithm is used to move the spring smoothly while the player is holding.
    //A quadratic out ease is used to simulate the increasing tension by moving the spring less over time.
    private IEnumerator MoveSpring() {
        float perc = currentDistance / (endPos - startPos);
        float time = prevTime;
        lerping = true;
        //Up to ten times, the 'tense' sound effect is played to mimic a spring creaking.
        //If it is played, it sets 'played' to false, causing the 'release' sound to play on release.
        if (time % tenseNum < 0.01 && time >= 0.1) {
            played = false;
            GetComponent<AudioSource>().PlayOneShot(tense);
            tenseNum += 0.1f;
        }
        if (time < perc) {
            float lerpPerc = Easing.Quadratic.Out(time);
            float lerpFloat = Mathn.Lerp(0f, (endPos - startPos), lerpPerc);
            transform.position = new Vector3(transform.position.x, startPos + lerpFloat, transform.position.z);
            time += released ? Time.deltaTime * 2 : Time.deltaTime;
            yield return null;
        }
        lerping = false;
        prevTime = time;
    }
}
