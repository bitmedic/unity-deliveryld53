using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScoreController : MonoBehaviour
{
    public TextMeshProUGUI textMoney;
    public TextMeshProUGUI textPegel;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textRank;
    public TextMeshProUGUI textTier;

    private bool finished = false;

    private Leaderboard leaderboard;

    private void Awake()
    {
        leaderboard = (Leaderboard) FindObjectOfType(typeof(Leaderboard));
    }

    private void Start()
    {

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (finished && Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    public async void ShowFinalScore(int money, float pegel)
    {
        this.gameObject.SetActive(true);
        
        int score = money * (int)Mathf.Round(pegel * 100);

        var entry = await leaderboard.SetPlayerScore(score);
        var scores = await leaderboard.GetHighScores();

        textRank.text = $"Your best: {entry.Score}";
        textTier.text = $"Rank: {entry.Tier} (Pos {entry.Rank + 1}/{scores.Total})";

        textMoney.text = money.ToString();
        textPegel.text = ((int)Mathf.Round(pegel * 100)).ToString();

        textScore.text = score.ToString();

        Invoke("setFinished", 5f);
    }

    private void setFinished()
    {
        finished = true;
    }
}
