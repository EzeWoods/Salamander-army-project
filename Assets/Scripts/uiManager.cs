using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class uiManager : MonoBehaviour
{
    public static uiManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin, menuLose;
    [SerializeField] GameObject damageScreen;
    [SerializeField] GameObject screenMessage;
    [SerializeField] TMP_Text scoreCountText;
    [SerializeField] TMP_Text scoreGoalText;
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] public TMP_Text ammoLabel;
    [SerializeField] public TMP_Text ammoLabelText;
    [SerializeField] GameObject hitmarker;
    [SerializeField] GameObject reloadPrompt;
    [SerializeField] GameObject optionsMenu;
    public Image playerHPBar;

    float timeScaleOrig;
    public bool isPaused;

    int scoreGoal;
    public int enemiesLeft;
    public bool hadPrompt;

    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            //if (menuActive == reloadPrompt)
            //{
            //    hideReloadPrompt();
            //    hadPrompt = true;
            //}
            //if (hadPrompt)
            //{
            //    showReloadPrompt();
            //    hadPrompt = false;
            //}

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

        if (optionsMenu.active)
            optionsMenu.SetActive(false);

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
        if (gameManager.instance)
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

    public IEnumerator hitMarkerDisplay()
    {
        instance.hitmarker.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        instance.hitmarker.SetActive(false);
    }

    public void showReloadPrompt()
    {
        reloadPrompt.SetActive(true);
    }

    public IEnumerator showScreenMessage()
    {
        screenMessage.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        screenMessage.SetActive(false);
    }

    public void hideReloadPrompt()
    {
        reloadPrompt.SetActive(false);
    }

    public void hidePrompt()
    {
        if (menuActive != null)
        {
            menuActive.SetActive(false);
            menuActive = null;
        }
    }

    public void loadNextLevel()
    {
        Time.timeScale = 1f; // Resume the game if it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Load the next level
    }


}
