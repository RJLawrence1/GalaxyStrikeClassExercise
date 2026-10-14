using TMPro;
using UnityEngine;

// Put this on an empty GameObject called "ScoreManager".
public class ScoreManager : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText; // drag your Score text here

    int score = 0;

    void Start()
    {
        UpdateText();
    }

    // "int amount" is the parameter. Target passes its scoreValue as the argument.
    public void AddScore(int amount)
    {
        score += amount;
        UpdateText();
    }

    void UpdateText()
    {
        scoreText.text = "Score: " + score;
    }
}
