using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//This script is on UI elements that need to be reset to stored values when enabled.
public class MenuReset : MonoBehaviour {
    private enum menuType { startButton, leaderboard, volSlider };
    [SerializeField] private menuType type;
    [SerializeField] private int level;
    
    private void OnEnable() {
        Reset();
    }

    public void Reset() {
        switch (type) {
            case menuType.startButton:
                if (level <= PlayerPrefs.GetInt("unlockedLevel", 0)) {
                    GetComponent<Image>().color = Color.white;
                } else {
                    GetComponent<Image>().color = Color.grey;
                }
                break;
            case menuType.leaderboard:
                TextMeshProUGUI names = transform.Find("Names").GetComponent<TextMeshProUGUI>();
                TextMeshProUGUI scores = transform.Find("Scores").GetComponent<TextMeshProUGUI>();
                names.text = PlayerPrefs.GetString("highScoreNames");
                scores.text = PlayerPrefs.GetString("highScores");
                break;
            case menuType.volSlider:
                GetComponent<Slider>().value = PlayerPrefs.GetFloat("volume", 0.5f);
                break;
        }
    }
}
