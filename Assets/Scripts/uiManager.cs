using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin, menuLose;
    [SerializeField] GameObject damageScreen;
    [SerializeField] TMP_Text scoreCountText;
    [SerializeField] TMP_Text scoreGoalText;
    [SerializeField] TMP_Text enemiesLeftText;
    public Image playerHPBar;

    float timeScaleOrig;
    public bool isPaused;

    int scoreGoal;
    int enemiesLeft;

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        uiManager.instance.updateScoreToWin(winZone.instance.scoreWinVar);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }

        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);

    }

    public void youWin()
    {
        statePause();
        menuActive = menuWin;
        menuActive.SetActive(true);
    }

    public void updateGameGoal(int amount)
    {
        if(gameManager.instance)
        {
            gameManager.instance.playerScript.score += amount;
            scoreCountText.text = gameManager.instance.playerScript.score.ToString("F0");
        }

    }

    public void updateScoreToWin(int amount)
    {
        scoreGoal += amount;
        scoreGoalText.text = scoreGoal.ToString("F0");
    }

    public void updateEnemiesInScene(int amount)
    {
        enemiesLeft += amount;
        enemiesLeftText.text = enemiesLeft.ToString("F0");
    }

    public IEnumerator flashScreenDamage()
    {
        instance.damageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        instance.damageScreen.SetActive(false);
    }


}
