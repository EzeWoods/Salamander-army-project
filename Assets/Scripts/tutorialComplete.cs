using UnityEngine;
using UnityEngine.SceneManagement;

public class tutorialComplete : MonoBehaviour
{
    public void OnTututorialDone()
    {
        SceneManager.LoadScene("Level 1");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnTututorialDone();
        }
    }
}
