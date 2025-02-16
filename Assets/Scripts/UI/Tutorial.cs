using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This script is on the parent of the 'tutorial' images that appear on the canvas.
//It controls their appearance and movement, to guide the player.
public class Tutorial : MonoBehaviour
{
    private GameObject[] images = new GameObject[3];
    private bool flipperExplained;

    private void Awake() {
        //images = FindObjectsOfType<GameObject>(true);
        //The above comment was used to find the children, as it would circumvent needing to disable them on start.
        //I removed it as it was causing issues, due to selecting every object in the scene.
        images[0] = GameObject.Find("TutorialCursor");
        images[0].transform.position = new Vector3(840, 558, 0);
        images[1] = GameObject.Find("TutorialCursor (1)");
        images[2] = GameObject.Find("Dashed Line");
        SetAll(false);
    }

    private void SetAll(bool state) {
        foreach (var image in images) {
            image.SetActive(state);
        }
    }

    //The '-tutorial' and 'end-' functions are called by events on the Ball script.
    public void LauncherTutorial() {
        SetAll(false);
        images[0].SetActive(true);
        images[0].GetComponent<Animator>().SetTrigger("Launcher");
    }

    public void EndLauncher() {
        images[0].SetActive(false);
    }

    public void FlipperTutorial() {
        //Is only run at the very beginning, not activating if the player has used the flippers before.
        if (flipperExplained) {
            return;
        }
        SetAll(true);
        images[0].transform.position = new Vector3(840, 558, 0);
        images[0].GetComponent<Animator>().SetTrigger("Flipper");
        images[1].transform.position = new Vector3(280, 558, 0);
        images[1].GetComponent<Animator>().SetTrigger("Flipper");
    }

    public void EndFlipper(bool flipped) {
        //'flipped' is used to determine if the flippers have been used or if the ball bounced into the launcher zone before then.
        if (flipperExplained || !flipped) {
            return;
        }
        flipperExplained = true;
        SetAll(false);
    }
}
