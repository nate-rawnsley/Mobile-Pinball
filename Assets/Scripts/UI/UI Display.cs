using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

//This script is on the in-level UI and controls its behaviour throughout.
public class UIDisplay : MonoBehaviour
{
    [SerializeField] private Ball ball;
    private int increment = 1;
    private TextMeshProUGUI scoreDisplay;
    private TextMeshProUGUI livesDisplay;
    private GameObject endDisplay;
    private List<GameObject> endChildren = new List<GameObject>();
    private int[] lastVals = { 0, 3 };
    [SerializeField] private GameObject scoreFrame;

    private void Awake() {
        PlayerPrefs.SetInt("Score", 0);
        scoreDisplay = transform.Find("Score Display").GetComponent<TextMeshProUGUI>();
        livesDisplay = transform.Find("Lives Display").GetComponent<TextMeshProUGUI>();
        endDisplay = GameObject.Find("End Display");
        endDisplay.SetActive(false);
        foreach (Transform child in endDisplay.transform) {
            endChildren.Add(child.gameObject);
        }
    }

    //Displays the current lives and score every frame.
    private void FixedUpdate() {
        if (PlayerPrefs.GetInt("Score") != lastVals[0]) {
            StartCoroutine(DisplayScore());
        }
        if (PlayerPrefs.GetInt("Lives") != lastVals[1]) {
            livesDisplay.text = "x " + ball.GetLives().ToString("00");
        }
    }

    //Whenever the score is updated, the display is updated and a small canvas is initialised on the ball.
    private IEnumerator DisplayScore() {
        int score = PlayerPrefs.GetInt("Score");
        Transform ballObject = ball.transform;
        GameObject displayFrame = Instantiate(scoreFrame, ballObject.position, ballObject.rotation);
        TextMeshProUGUI display = displayFrame.transform.Find("Score Text").GetComponent<TextMeshProUGUI>();
        display.text = (PlayerPrefs.GetInt("Score") - lastVals[0]).ToString();
        lastVals[0] = score;
        scoreDisplay.text = score.ToString();
        //Every 2500 points, an extra life is given (and text is displayed to show this).
        if (score >= increment * 2500) {
            increment++;
            ball.ExtraLife();
        } else {
            displayFrame.transform.Find("Extra Life").gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        Destroy(displayFrame);
    }

    //From an event on Ball, starts the process of the game over sequence.
    public void EndGame() {
        StartCoroutine(EndDisplay());
    }

    private IEnumerator EndDisplay() {
        endDisplay.SetActive(true);
        GameObject gameOver = endChildren[0];
        scoreDisplay.enabled = false;
        bool state = true;
        for (int k = 0; k < 5;  k++) { // creates a flashing effect
            gameOver.SetActive(state);
            yield return new WaitForSeconds(1);
            state = !state;
        }
        gameOver.GetComponent<Animator>().SetTrigger("Pan");
        yield return new WaitForSeconds(2);
        //Checks if the score is enough to get onto the leaderboard.
        string[] highScores = PlayerPrefs.GetString("highScores").Split('\n');
        int score = PlayerPrefs.GetInt("Score");
        bool newHigh = false;
        int position = 0;
        for (int i = 0; i < highScores.Length; i++) {
            if (score >= int.Parse(highScores[i])) {    
                for (int j = highScores.Length - 1; j < i; j--) {
                    highScores[j] = highScores[j - 1];
                }
                highScores[i] = score.ToString();
                position = i + 1;
                newHigh = true;
                break;
            }
        }
        if (newHigh) {
            //Allows the player to input their name and save their score to the leaderboard.
            GameObject.Find("Leaderboard Adding").GetComponent<LeaderInput>().Active(position, highScores);
        } else {
            FinalDisplay(score);
        }
    }

    //Displays the end screen and final buttons.
    public void FinalDisplay(int score) {
        endChildren[1].SetActive(true);
        GameObject endScore = endChildren[2];
        endScore.SetActive(true);
        endScore.GetComponent<TextMeshProUGUI>().text = score.ToString();
        endChildren[3].SetActive(true);
    }

    //Called by event on the 'return to main menu' button.
    public void Return() {
        SceneManager.LoadScene("MainMenu");
    }
}
