using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] int numToSpawn;
    [SerializeField] Transform[] spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawn());
    }

    IEnumerator spawn()
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
