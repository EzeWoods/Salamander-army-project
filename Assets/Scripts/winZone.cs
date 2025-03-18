using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winZone : MonoBehaviour
{
    public static winZone instance;

    [SerializeField] bool needKillsToWin;
    [SerializeField] int scoreToWin;
    [SerializeField] GameObject doorToOpen;
    [SerializeField] GameObject exitTextPrompt;
    bool inZone = false;
    public int scoreWinVar;

    private void Awake()
    {
        instance = this;
        scoreWinVar = scoreToWin;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.instance.playerScript.score >= scoreToWin)
        {
            doorToOpen.SetActive(false);
            exitTextPrompt.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;

            if(needKillsToWin)
            {
                if(gameManager.instance.playerScript.score >= scoreToWin)
                {
                    uiManager.instance.youWin();
                }    
                
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }
}
