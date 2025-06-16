using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class tmpPlayer : MonoBehaviour
{
    private bool isAimming;
    private bool isFiring;

    private PlayerMovement playerMovement;
    private PlayerAttackAction playerAttackAction;
    private PlayerEquipment playerEquipment;

    private Rigidbody rb;
    private Animator animator;

    private Camera mainCam;

    private void Awake()
    {
        isAimming = false;
        isFiring = false;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        playerMovement = GetComponent<PlayerMovement>();
        playerAttackAction = GetComponent<PlayerAttackAction>();
        playerEquipment = GetComponent<PlayerEquipment>();
    }

    private void Start()
    {
        mainCam = Camera.main;

        playerMovement.Initailize(rb, animator, mainCam);
        playerAttackAction.Initailize(rb, animator, mainCam);
    }

    private void FixedUpdate()
    {
        playerMovement.MovementFixedUpdate();
    }

    private void Update()
    {
        isFiring = Input.GetMouseButton(0);
        isAimming = Input.GetMouseButton(1);

        playerMovement.MovementUpdate(isAimming || isFiring);

        if (isFiring)
        {
            playerAttackAction.FireGun();
        }
        animator.SetBool("isAimming", isAimming);
    }

    private void LateUpdate()
    {
        if (isAimming)
        {
            mainCam.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40f, 0.5f);
        }
        else
        {
            mainCam.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, 0.5f);
        }
        playerMovement.RotateLateUpdate(isAimming || isFiring);
    }
}
