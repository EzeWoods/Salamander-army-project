using UnityEngine;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    public GameObject player;
    public playerController playerScript;
    [SerializeField] public int scoreToWin;
    public int enemiesAlive;
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        uiManager.instance.updateScoreToWin(scoreToWin);
    }

    void Update()
    {

    }
}
