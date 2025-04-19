using UnityEngine;
using TMPro;

public class HighScoreNumberMainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int highscore = PlayerPrefs.GetInt("Highscore");
        highScoreNumber.text = highscore.ToString();
    }

}
