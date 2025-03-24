using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorFunction : MonoBehaviour
{
    [SerializeField] int scoreToOpen;
    [SerializeField] bool showMessage;

    void Update()
    {
        if(gameManager.instance.playerScript.score == scoreToOpen)
        {
            this.gameObject.SetActive(false);
        }
    }
}
