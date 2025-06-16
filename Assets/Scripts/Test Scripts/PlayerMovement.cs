using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveKeyDir;
    private float walkSpeed;    // �ȱ� �ӵ�
    private float runSpeed;     // �޸��� �ӵ�
    private float moveSpeed;    // ���� �̵� �ӵ�
    private bool isRunning;

    private float moveElapsedTime;  // �����̴� �ð� ����

    private const int maxJumpCount = 2; // �ִ� ���� Ƚ��
    private int jumpCount;              // ���� ���� Ƚ��
    private float jumpPower;            // ������
    private bool isJumpKeyInput;

    private bool isGrounded;
    private float aerialElapsedTime;

    private bool isAimming;
    private bool isShooting;

    private Rigidbody rb;
    private Animator animator;
    private Camera mainCam;

    private void Awake()
    {
        moveKeyDir = Vector3.zero;
        isRunning = false;
        walkSpeed = 5f;
        runSpeed = 10f;
        moveSpeed = walkSpeed;

        moveElapsedTime = 0f;

        jumpCount = 0;
        jumpPower = 4f;
        isJumpKeyInput = false;

        isGrounded = true;
        aerialElapsedTime = 0f;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            jumpCount = 0;
            Debug.Log("�ٴڰ� �浹 ��");
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        Debug.Log("�浹 ���" + collision.gameObject.name);
    }

    public void Initailize(Rigidbody rigidbody, Animator animator, Camera camera)
    {
        rb = rigidbody;
        this.animator = animator;
        mainCam = camera;
    }

    public void MovementFixedUpdate()
    {
        Vector3 worldKeyDir = transform.TransformDirection(moveKeyDir);
        rb.MovePosition(rb.position + worldKeyDir * moveSpeed * Time.fixedDeltaTime);

        if (isJumpKeyInput)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            ++jumpCount;
            isJumpKeyInput = false;
        }
    }

    public void MovementUpdate(bool isRifleAction)
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        moveKeyDir = new Vector3(horizontal, 0f, vertical).normalized;
        isRunning = Input.GetKey(KeyCode.LeftShift);
        moveSpeed = isRunning || !isRifleAction ? runSpeed : walkSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            isJumpKeyInput = true;
        }

        animator.SetFloat("vertical", vertical);
        animator.SetFloat("horizontal", horizontal);
        animator.SetBool("isMoveKeyInput", moveKeyDir.magnitude > 0f);
        animator.SetBool("isRunning", isRunning);

        animator.SetBool("isJumpKeyInput", isJumpKeyInput);
        animator.SetFloat("velocityY", rb.velocity.y);
        if (isGrounded)
        {
            aerialElapsedTime = 0f;
        }
        else
        {
            animator.SetBool("isLanding", Physics.Raycast(gameObject.transform.position, Vector3.down, 1.1f));
            aerialElapsedTime += Time.deltaTime;
        }
        animator.SetFloat("aerialElapsedTime", aerialElapsedTime);
    }

    public void RotateLateUpdate(bool isRifleAction)
    {
        Vector3 camForward = mainCam.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Quaternion targetRot = Quaternion.LookRotation(camForward);

        if (isRifleAction)
        {
            rb.MoveRotation(targetRot);
        }
        else
        {
            if (moveKeyDir.magnitude > 0f)
            {

                if (moveElapsedTime < 3f)
                {
                    moveElapsedTime += Time.deltaTime;
                    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, moveElapsedTime / 3f));
                }
                else
                {
                    rb.MoveRotation(targetRot);
                }
            }
            else
            {
                moveElapsedTime = 0f;
            }
        }
    }
}
