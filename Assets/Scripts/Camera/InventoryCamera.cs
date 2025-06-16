using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryCamera : MonoBehaviour
{
    private Player player;
    private Vector3 offsetPos;

    private void Awake()
    {
        offsetPos = new Vector3(0f, 1.5f, 3f);
    }

    private void Start()
    {
        player = PlayManager.instance.Player;
    }

    private void OnEnable()
    {
        if (player != null)
        {
            transform.position = player.transform.position + offsetPos;
            transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles * -1f);
        }
    }

    private void LateUpdate()
    {
        Quaternion camRotation = Quaternion.Euler(0f, player.transform.rotation.eulerAngles.y, 0f);
        Vector3 pos = player.transform.position + camRotation * offsetPos;
        transform.position = pos;
        transform.LookAt(player.transform.position + Vector3.up);
    }
}
