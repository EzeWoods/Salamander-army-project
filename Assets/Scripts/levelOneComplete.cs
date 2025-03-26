using UnityEngine;
using UnityEngine.SceneManagement;

public class levelOneComplete : MonoBehaviour
{
    public void LeveOneDone()
    {
        SceneManager.LoadScene("Level 2");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LeveOneDone();
        }
    }
}
