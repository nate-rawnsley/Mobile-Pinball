using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

//This is on both the main menu and pause menu parent objects and controls their operations.
//Most scripts are called from events, which are triggered by UI buttons.
public class MenuHandler : MonoBehaviour
{
    [SerializeField] private bool main; // determines whether it is the main or pause menu
    private GameObject[] frames = new GameObject[6];
    public UnityEvent progressReset;

    private void Awake() {
        if (!main) {
            return;
        }
        frames[0] = transform.Find("Main Menu").gameObject;
        frames[1] = transform.Find("Level Select").gameObject;
        frames[2] = transform.Find("Leaderboard").gameObject;
        frames[3] = transform.Find("Options").gameObject;
        frames[4] = transform.Find("Erase Confirm").gameObject;
        frames[5] = transform.Find("Erased Notification").gameObject;
        if (PlayerPrefs.GetString("highScores", "None") == "None") {
            string[] highNames = { "N/A", "N/A", "N/A", "N/A", "N/A" };
            string[] highScores = { "0", "0", "0", "0", "0" };
            PlayerPrefs.SetString("highScoreNames", string.Join("\n", highNames));
            PlayerPrefs.SetString("highScores", string.Join("\n", highScores));
        }
    }

    public void ShowFrame(int show) {
        frames[show].SetActive(true);
    }

    public void HideFrame(int hide) {
        frames[hide].SetActive(false);
    }

    public void StartLevel (int level) {
        if (level <= PlayerPrefs.GetInt("unlockedLevel", 0)) {
            PlayerPrefs.SetInt("curLevel", level);
            SceneManager.LoadScene("Main");
        }
    }

    public void VolumeUpdate(float value) {
        PlayerPrefs.SetFloat("volume", value);
    }

    public void ResetProgress() {
        PlayerPrefs.SetInt("unlockedLevel", 0);
        string[] highNames = { "N/A", "N/A", "N/A", "N/A", "N/A" };
        string[] highScores = { "0", "0", "0", "0", "0" };
        PlayerPrefs.SetString("highScoreNames", string.Join("\n", highNames));
        PlayerPrefs.SetString("highScores", string.Join("\n", highScores));
        progressReset.Invoke();
    }

    public void Pause() {
        frames[0] = gameObject;
        frames[1] = transform.Find("Confirmation").gameObject;
        Time.timeScale = 0f;
        ShowFrame(0);
    }

    public void Resume() {
        Time.timeScale = 1f;
        HideFrame(0);
    }

    public void MainMenu() {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
        Application.Quit();
    }
}
