using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script is on the ball and controls its interactions (primarily respawning and activating the flippers and launchers)
public class Ball : MonoBehaviour
{
    [SerializeField] private Launcher launcher;
    [SerializeField] private Vector3[] respawns;
    [SerializeField] private GameObject particles;
    public bool launcherActive;
    private int lives = 2;
    public bool dead;
    public UnityEvent gameOver;
    public UnityEvent launcherZone;
    public UnityEvent mainZone;

    //The 'respawns' array holds the positions where the ball will respawn, depending on what level was selected.
    //At the start of the level, (as well as on death), the ball's position is reset to the relevant location.
    private void Awake() {
        transform.position = respawns[PlayerPrefs.GetInt("curLevel", 0)];
    }

    //This controls whether the launcher or the flippers are being controlled by the user's input.
    //Within the levels are triggers that determine which should be controlled at a given time.
    //The events are used to activate & deactivate the visual tutorials, as well as change the controls.
    private void OnTriggerStay(Collider other) {
        if (other.transform.CompareTag("MainZone") && launcherActive) {
            launcherActive = false;
            launcherZone.Invoke();
        } else if (other.transform.CompareTag("LauncherZone") && !launcherActive) {
            launcherActive = true;
            mainZone.Invoke();
        }
    }

    //Sound and particles are used whenever the ball collides with an object to add flair to the project.
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Untagged")&& !dead) {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().time = 0.2f;
        }
        StartCoroutine(MakeSmoke());
    }

    private IEnumerator MakeSmoke() {
        GameObject smoke = Instantiate(particles, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(smoke);
    }

    public int GetLives() {
        return lives;
    }

    public void ExtraLife() {
        lives++;
    }

    public bool Death(int level) {
        //On death, an event is used to trigger the UI 'Game Over' sequence and disable input.
        if (lives == 0) {
            gameOver.Invoke();
            dead = true;
            return true;
        }
        lives--;
        transform.position = respawns[level];
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        return false;
    }
}
