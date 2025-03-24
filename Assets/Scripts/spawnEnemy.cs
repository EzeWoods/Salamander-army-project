using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnEnemy : MonoBehaviour
{
    public static spawnEnemy instance;
    [SerializeField] Transform spawnPos;

    [SerializeField] GameObject bossToSpawn;
    [SerializeField] bool once;

    public bool enemySpawned;

    void Awake()
    {
        instance = this;
    }

    // X 41.40223 Y -13.92826 Z 42.24126
    void spawnBoss(Transform spawnPos)
    {
        enemySpawned = true;
        Instantiate(bossToSpawn, spawnPos.position, spawnPos.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(!enemySpawned && once)
            {
                spawnBoss(spawnPos);
            }
            
        }

    }
}
