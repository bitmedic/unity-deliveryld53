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
    public GameObject player_step1;
    public GameObject player_step2;
    public GameObject customer;
    public GameObject bar;
    public GameObject draftZone;

    public Button backButton;

    // Update is called once per frame
    void Start()
    {
        ShowTutorisalStep();
    }

    private void Update()
    {
        //if (tutorialStep == 2)
        //{
        //    if (player_step2.transform.position.x <= 16)
        //        player_step2.transform.position += new Vector3(4,0,0) *  Time.deltaTime;
        //    else
        //        player_step2.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        //}
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

        player_step1.gameObject.SetActive(false);
        player_step2.gameObject.SetActive(false);
        customer.gameObject.SetActive(false);
        bar.gameObject.SetActive(false);
        draftZone.gameObject.SetActive(false);


        textTutorialStep.text = tutiorialSteps[tutorialStep].tutorialStpeText;

        //switch (tutorialStep)
        //{
        //    case (0): // walking
        //        {
        //            textTutorialStep.text = "Walk around\nusing\n\n\nW/A/S/D";
        //            //player_step1.SetActive(true);
        //            //player_step1.GetComponentInChildren<Animator>().SetBool("isWalking", true);
        //            break;
        //        }
        //    case (1): // take order
        //        {
        //            textTutorialStep.text = "Spot annoying customer that want to order drinks";
        //            //customer.GetComponent<Guest>().DecideOrder();
        //            //customer.SetActive(true);
        //            break;
        //        }
        //    case (2): // take order
        //        {
        //            textTutorialStep.text = "(Carefully) Bump into the customer to take their order";
        //            //customer.GetComponent<Guest>().DecideOrder();
        //            //customer.SetActive(true);
        //            //player_step2.transform.position = new Vector3(6.6f, player_step2.transform.position.y, player_step2.transform.position.z);
        //            //player_step2.SetActive(true);
        //            //player_step2.GetComponentInChildren<Animator>().SetBool("isWalking", true);
        //            break;
        //        }
        //    case (3): // pour
        //        {
        //            textTutorialStep.text = "Go behind the bar to pour up to 5 drinks";
        //            //player_step2.GetComponentInChildren<Animator>().SetBool("isWalking", false);
        //            //bar.SetActive(true);
        //            //draftZone.SetActive(true);
        //            break;
        //        }
        //    case (4): // deliver
        //        {
        //            textTutorialStep.text = "Try not to drink all the drinks before they reach your customers or you'll get drunk before last call";
        //            break;
        //        }
        //}
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
