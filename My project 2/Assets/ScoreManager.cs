using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;

    public TMP_Text scoreText;

    private const string ScoreKey = "PlayerScore";

    void Awake()
    {
        // Load the saved score when the game starts
        score = PlayerPrefs.GetInt(ScoreKey, 0);
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore()
    {
        score++;

        // Save the score permanently
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();

        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}