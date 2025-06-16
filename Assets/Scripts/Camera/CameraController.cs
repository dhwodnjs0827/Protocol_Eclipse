using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Player player;
    private Transform camPlayerLookPoint;
    private Transform camNPCLookPoint;

    private float rotateSpeed; // 마우스 회전 속도
    private float rotateX;
    private float rotateY;
    private float minAngleY;  // 카메라가 아래로 움직일 수 있는 각도의 제한
    private float maxAngleY;   // 카메라가 위로 움직일 수 있는 각도의 제한

    private float distance;
    private float minDistance;
    private float maxDistance;

    private void Awake()
    {
        player = PlayManager.instance.Player;
        camPlayerLookPoint = player.gameObject.transform.Find("CameraLookPoint");

        rotateSpeed = 50f;
        rotateX = 0f;
        rotateY = 0f;
        minAngleY = -40f;
        maxAngleY = 40f;
    }

    void Update()
    {
        if (!UIManager.Instance.IsUIOpen())
        {
            rotateX -= Input.GetAxis("Mouse Y") * rotateSpeed * Time.deltaTime;  // 마우스 Y 축으로 카메라 회전
            rotateX = Mathf.Clamp(rotateX, minAngleY, maxAngleY); // Y축 각도를 제한
        }
        else
        {
            if (UIManager.Instance.NPCUI.gameObject.activeSelf)
            {
                camNPCLookPoint = PlayManager.instance.InteractionNPC.InteractCamPos;
            }
        }
    }

    void LateUpdate()
    {
        if (!UIManager.Instance.IsUIOpen())
        {
            // 카메라 위치와 회전 계산
            rotateY = player.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(rotateX, rotateY, 0);
            Vector3 direction = new Vector3(0.8f, 1.6f, -3f);
            Vector3 position = player.transform.position + rotation * direction;
            transform.position = position;
            transform.LookAt(camPlayerLookPoint.position);  // 플레이어를 항상 바라보도록 설정
        }
        else
        {
            if (UIManager.Instance.NPCUI.gameObject.activeSelf)
            {
                float camSpeed = 5f;
                transform.position = Vector3.Lerp(transform.position, camNPCLookPoint.position, camSpeed * Time.deltaTime);
                transform.rotation = Quaternion.Lerp(transform.rotation, camNPCLookPoint.rotation, camSpeed * Time.deltaTime);
            }
        }
    }
}
