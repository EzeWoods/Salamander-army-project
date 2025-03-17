using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class winZone : MonoBehaviour
{
    [SerializeField] bool needKillsToWin;
    [SerializeField] int scoreToWin;
    bool inZone = false;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;

            if(needKillsToWin && scoreToWin == gameManager.instance.playerScript.score)
            {
                uiManager.instance.youWin();
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
