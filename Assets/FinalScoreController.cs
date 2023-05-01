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

    private bool finished = false;


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

    public void ShowFinalScore(int money, float pegel)
    {
        this.gameObject.SetActive(true);

        textMoney.text = money.ToString();
        textPegel.text = ((int)Mathf.Round(pegel * 100)).ToString();

        int score = money * (int)Mathf.Round(pegel * 100);
        textScore.text = score.ToString();

        Invoke("setFinished", 5f);
    }

    private void setFinished()
    {
        finished = true;
    }
}
