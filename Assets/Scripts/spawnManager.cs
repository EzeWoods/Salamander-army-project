using System.Collections;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] Transform[] spawnPos;
    [SerializeField] float waveDelay;
    [SerializeField] int totalWaves; // 2
    [SerializeField] AudioSource aud;
    int currWave = 0;


    bool readyToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        aud.volume = menuManager.instance.playerSettings.volumePercent;
        StartCoroutine(spawnWaves());
    }

    IEnumerator spawnWaves()
    {
        aud.PlayOneShot(aud.clip, aud.volume);

        while (currWave < totalWaves)
        {
            yield return StartCoroutine(spawnEnts());

            currWave++;

            yield return new WaitUntil(() => gameManager.instance.enemiesAlive <= 0);

            if (currWave < totalWaves)
            {
                yield return new WaitForSeconds(waveDelay);
            }
        }

    }

    IEnumerator spawnEnts()
    {
        for (int i = 0; i < numToSpawn; i++)
        {
            yield return new WaitForSeconds(0.3f);

            int spawnInt = Random.Range(0, spawnPos.Length);
            GameObject enemyRand = objectToSpawn[Random.Range(0, objectToSpawn.Length)];

            Instantiate(enemyRand, spawnPos[spawnInt].position, spawnPos[spawnInt].rotation);

        }

    }
}
