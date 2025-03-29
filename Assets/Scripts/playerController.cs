using Kinemation.SightsAligner;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] AudioSource aud;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] GameObject minimap;


    [Header("----- Stats -----")]
    [SerializeField][Range(1, 10)] int HP;
    [SerializeField][Range(1, 5)] int speed;
    [SerializeField][Range(2, 5)] int sprintMod;
    [SerializeField][Range(1, 3)] int jumpMax;
    [SerializeField][Range(5, 20)] int jumpSpeed;
    [SerializeField][Range(15, 40)] int gravity;

    [Header("----- Gun Stats -----")]
    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField][Range(0.0f, 5.0f)] float reloadSpeed;
    [SerializeField][Range(0, 999)] int maxStoredAmmo;
    [SerializeField][Range(0, 40)] int maxCurrentAmmo;

    [Header("~~~ Audio ~~~")]
    [SerializeField] AudioClip[] audSteps;
    [SerializeField] AudioClip[] audHurt;
    [SerializeField] AudioClip[] audJump;

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;
    public int score;
    int currentAmmo;
    int storedAmmo;

    bool isSprinting;
    bool isShooting;

    private bool isReloading;

    bool isPlayingSteps;

    void Start()
    {
        HPOrig = HP;
        currentAmmo = maxCurrentAmmo;
        storedAmmo = maxStoredAmmo;
        uiManager.instance.updateGameGoal(0);
        updatePlayerHPBar();
        updateAmmoUI();
        minimap.SetActive(true);
    }

    void Update()
    {
        if(!uiManager.instance.isPaused)
        {
            movement();
        }

        sprint();
    }

    void movement()
    {
        if (controller.isGrounded)
        {
          if (moveDir.magnitude > 0.3f && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }

            jumpCount = 0;
            playerVel = Vector3.zero;
        }

        moveDir = (transform.right * Input.GetAxis("Horizontal")) +
                   (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(moveDir * speed * Time.deltaTime);

        jump();

        controller.Move(playerVel * Time.deltaTime);
        playerVel.y -= gravity * Time.deltaTime;

        if ((controller.collisionFlags & CollisionFlags.Above) != 0)
        {
            playerVel = Vector3.zero;
        }

        if (Input.GetButton("Fire1") && !isShooting && currentAmmo > 0 && !isReloading)
        {
            StartCoroutine(shoot());
        }

        if(!isReloading && currentAmmo < storedAmmo)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(reload());
            }
            
            if(currentAmmo <= 0)
            {
                uiManager.instance.showReloadPrompt();
                
            }
        }
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;

        aud.PlayOneShot(audSteps[Random.Range(0, audSteps.Length)], menuManager.instance.playerSettings.volumePercent);

        if (!isSprinting)
            yield return new WaitForSeconds(0.5f);
        else
            yield return new WaitForSeconds(0.3f);

        isPlayingSteps = false;
    }

    IEnumerator shoot()
    {
        isShooting = true;

        currentAmmo--;
        updateAmmoUI();


       
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreMask))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
                StartCoroutine(uiManager.instance.hitMarkerDisplay());
            }

        }

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }

    IEnumerator reload()
    {
        if (storedAmmo <= 0 || currentAmmo == maxCurrentAmmo)
            yield break;

        isReloading = true;
        //uiManager.instance.hideReloadPrompt();
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadSpeed);

        int ammoNeeded = maxCurrentAmmo - currentAmmo;
        int ammoToReload = Mathf.Min(ammoNeeded, storedAmmo);

        currentAmmo += ammoToReload;
        storedAmmo -= ammoToReload;

        updateAmmoUI();
       
        isReloading = false;
    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], menuManager.instance.playerSettings.volumePercent);
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
            isSprinting = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        aud.PlayOneShot(audHurt[Random.Range(0, audHurt.Length)], menuManager.instance.playerSettings.volumePercent);
        updatePlayerHPBar();
        StartCoroutine(uiManager.instance.flashScreenDamage());

        if (HP <= 0)
        {
            uiManager.instance.youLose();
        }
    }

    public void updatePlayerHPBar()
    {
        uiManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void updateAmmoUI()
    {
        uiManager.instance.ammoLabel.text = "/" + storedAmmo.ToString("F0");
        uiManager.instance.ammoLabelText.text = currentAmmo.ToString("F0");
        uiManager.instance.hideReloadPrompt();
    }

}
