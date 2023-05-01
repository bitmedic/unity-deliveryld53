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

    private Animator animator;

    private float timer = 10;

    // Update is called once per frame
    void Start()
    {
        ShowTutorisalStep();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (timer <= 0)
        {
            NextStep();
            timer = 5;
        }

        timer -= Time.deltaTime;
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

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        animator.SetTrigger("NextStep");
        //textTutorialStep.text = tutiorialSteps[tutorialStep].tutorialStpeText;

    }

    public void SetTutorialText()
    {
        textTutorialStep.text = tutiorialSteps[tutorialStep].tutorialStpeText;
    }

    public void NextStep()
    {
        tutorialStep++;
        if (tutorialStep >= tutiorialSteps.Count)
        {
            tutorialStep = 0;
        }
        timer = 5;
        ShowTutorisalStep();
    }

    public void PrevStep()
    {
        tutorialStep--;
        if (tutorialStep < 0)
        {
            tutorialStep = tutiorialSteps.Count - 1;
        }
        timer = 5;
        ShowTutorisalStep();
    }


    public void StartGame()
    {
        SceneManager.LoadScene("BarTestScene");
    }
}
