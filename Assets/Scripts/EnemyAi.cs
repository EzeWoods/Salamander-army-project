using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class enemyAI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;

    [SerializeField] Transform shootPos, headPos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int FOV;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDist;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject itemDrop;
    [SerializeField] float shootRate;
    [SerializeField] bool isBoss;

    float angleToPlayer;
    float stoppingDistOrig;

    bool isShooting;
    bool playerInRange;
    bool isRoaming;

    Color colorOrig;

    Vector3 playerDir;
    Vector3 startingPos;

    Coroutine co;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        //gameManager.instance.updateGameGoal(1);
        uiManager.instance.updateEnemiesInScene(1);
        gameManager.instance.enemiesAlive++;
        stoppingDistOrig = agent.stoppingDistance;
        startingPos = transform.position;

        co = StartCoroutine(roam());
    }

    // Update is called once per frame
    void Update()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeed = anim.GetFloat("Speed");

        anim.SetFloat("Speed", Mathf.MoveTowards(animSpeed, agentSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && canSeePlayer())
        {
            if (!isRoaming && agent.remainingDistance < 0.01f)
            {
                co = StartCoroutine(roam());
            }
        }

        else if (!playerInRange)
        {
            if (!isRoaming && agent.remainingDistance < 0.01f)
            {
                co = StartCoroutine(roam());

            }
        }
    }

    IEnumerator roam()
    {
        isRoaming = true;

        yield return new WaitForSeconds(roamPauseTime);

        agent.stoppingDistance = 0;

        Vector3 randomPos = Random.insideUnitSphere * roamDist;

        randomPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);

        isRoaming = false;
    }
    //Can the enemy see the player?
    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);

        Debug.DrawRay(headPos.position, playerDir);

          RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            //I can see the player!!!

            if (hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {

                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();
                }

                if (!isShooting)
                {
                    StartCoroutine(shoot());
                }
                agent.stoppingDistance = stoppingDistOrig;
                return true;
            }
        }
        return false;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }

    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);

    }

    public void takeDamage(int amount)
    {
        Debug.Log("Enemy taking damage: " + amount);
        HP -= amount;

        agent.SetDestination(gameManager.instance.player.transform.position);

        if (co != null)
        {
            StopCoroutine(co);
            isRoaming = false;
        }

        
        StartCoroutine(flashRed());

        if (HP <= 0)
        {
            //Debug.Log("Enemy died, dropping item.");
            //Instantiate(itemDrop, transform.position + Vector3.up * 0.5f , Quaternion.identity);

            //gameManager.instance.playerScript.score++;
            uiManager.instance.updateEnemiesInScene(-1);
            gameManager.instance.enemiesAlive--;
            uiManager.instance.updateGameGoal(1);

            if(isBoss)
            {
                uiManager.instance.youWin();
                
            }

            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
