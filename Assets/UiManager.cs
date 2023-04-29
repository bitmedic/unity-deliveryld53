using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public DrunkPlayer sourcePlayer;

    public Image pegelBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pegelBar.fillAmount = sourcePlayer.pegel;
    }
}
