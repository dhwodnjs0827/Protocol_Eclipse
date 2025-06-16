using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class tmpCameraControl : MonoBehaviour
{
    public PlayerMovement player;
    private Transform playerPoint;

    private Vector3 cameraPos;
    private Quaternion cameraRot;

    private float rotSpeed; // 마우스 회전 속도

    private float mouseRotX;
    private float mouseRotY;

    private const float minAngleX = -40f;  // 카메라가 아래로 움직일 수 있는 각도의 제한
    private const float maxAngleX = 40f;   // 카메라가 위로 움직일 수 있는 각도의 제한

    private const float minDistance = 1f;
    private const float maxDistance = 2.5f;
    private float distance;

    private void Awake()
    {
        rotSpeed = 1f;

        mouseRotX = 0f;
        mouseRotY = 0f;

        distance = maxDistance;
    }

    private void Start()
    {
        playerPoint = player.transform.Find("CameraPoint");
    }

    private void Update()
    {
        PlayerCameraUpdate();
    }

    private void LateUpdate()
    {
        PlayerCameraLateUpdate();
    }

    private void PlayerCameraUpdate()
    {
        mouseRotY += Input.GetAxis("Mouse X") * rotSpeed;
        mouseRotX += Input.GetAxis("Mouse Y") * rotSpeed * -1f;
        mouseRotX = Mathf.Clamp(mouseRotX, minAngleX, maxAngleX);
    }

    private void PlayerCameraLateUpdate()
    {
        cameraRot = Quaternion.Euler(new Vector3(mouseRotX, mouseRotY, 0f));
        cameraPos = new Vector3(0.8f, 0f, distance * -1f);
        gameObject.transform.position = playerPoint.position + cameraRot * cameraPos;
        gameObject.transform.rotation = cameraRot;
    }
}
