using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is on the leaderboard entry frame and controls updating the leaderboard.
public class LeaderInput : MonoBehaviour
{
    private List<GameObject> children = new List<GameObject>();
    private Image image;
    private string[] topScores = new string[5];
    private int currentPos;
    
    private void Awake() {
        foreach (Transform child in transform) {
            children.Add(child.gameObject);
        }
        image = GetComponent<Image>();
        SetAll(false);
    }

    private void SetAll(bool state) {
        image.enabled = state;
        foreach (var child in children) {
            child.SetActive(state);
        }
    }

    //Called from the UI Display script when the leaderboard must be updated.
    public void Active(int position, string[] highScores) {
        SetAll(true);
        currentPos = position;
        topScores = highScores;
        TextMeshProUGUI posDisplay = children[1].GetComponent<TextMeshProUGUI>();
        posDisplay.text = "Your Position: " + position.ToString();
    }

    //Called by event when the player clicks 'Submit' after writing their name.
    public void Submit() {
        string name = children[2].GetComponent<TMP_InputField>().text;
        name = name.PadRight(3, '-'); //ensures it is padded to 3 characters always
        string[] topNames = PlayerPrefs.GetString("highScoreNames").Split('\n');
        for (int i = topNames.Length - 1; i > currentPos - 2 && i > 0; i--) {
            topNames[i] = topNames[i - 1];
        }
        topNames[currentPos-1] = name;
        PlayerPrefs.SetString("highScoreNames", string.Join("\n", topNames));
        PlayerPrefs.SetString("highScores", string.Join("\n", topScores));
        SetAll(false);
        //Triggers the rest of the level end sequence on the UI Display script.
        transform.parent.gameObject.GetComponent<UIDisplay>().FinalDisplay(int.Parse(topScores[currentPos - 1]));
    }
}
