using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreMask;

    [Header("----- Stats -----")]
    [SerializeField][Range(1, 10)] int HP;
    [SerializeField][Range(1, 5)] int speed;
    [SerializeField][Range(2, 5)] int sprintMod;
    [SerializeField][Range(1, 3)] int jumpMax;
    [SerializeField][Range(5, 20)] int jumpSpeed;
    [SerializeField][Range(15, 40)] int gravity;

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;

    bool isSprinting;

    void Start()
    {
        HPOrig = HP;
    }

    void Update()
    {
        movement();
        sprint();
    }

    void movement()
    {
        if (controller.isGrounded)
        {
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

    }

    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
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

}
