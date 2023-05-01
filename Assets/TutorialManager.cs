using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public int tutorialStep;

    public List<TutorialStepSO> tutiorialSteps;

    public TextMeshProUGUI textTutorialStep;

    public Button backButton;

    // Update is called once per frame
    void Start()
    {
        ShowTutorisalStep();
    }

    private void Update()
    {
    }

    private void ShowTutorisalStep()
    {
        if (tutorialStep == 0)
        {
            backButton.gameObject.SetActive(false);
        }
        else
        {
            backButton.gameObject.SetActive(true);
        }


        textTutorialStep.text = tutiorialSteps[tutorialStep].tutorialStpeText;

    }

    public void NextStep()
    {
        tutorialStep++;
        if (tutorialStep >= tutiorialSteps.Count)
        {
            tutorialStep = 0;
        }
        ShowTutorisalStep();
    }

    public void PrevStep()
    {
        tutorialStep--;
        if (tutorialStep < 0)
        {
            tutorialStep = tutiorialSteps.Count - 1;
        }
        ShowTutorisalStep();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("BarTestScene");
    }
}
