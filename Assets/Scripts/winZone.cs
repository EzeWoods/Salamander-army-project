using UnityEngine;

public class winZone : MonoBehaviour
{
    public static winZone instance;

    [SerializeField] public int scoreToWin;
    [SerializeField] GameObject doorToOpen;
    [SerializeField] GameObject exitTextPrompt;
    [SerializeField] bool haveToDefeatBoss;
    bool inZone = false;

    public bool bossDefeated;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

        if (haveToDefeatBoss && bossDefeated)
        {
            exitTextPrompt.SetActive(true);

            if (gameManager.instance.playerScript.score >= gameManager.instance.scoreToWin)
            {
                uiManager.instance.youWin();
            }
        }
        else if (gameManager.instance.playerScript.score >= gameManager.instance.scoreToWin)
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

            if (gameManager.instance.playerScript.score >= gameManager.instance.scoreToWin)
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
