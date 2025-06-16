using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

// 플레이 씬의 전체적인 매니저
public class PlayManager : MonoBehaviour
{
    public static PlayManager instance;

    private Player player;

    private NPC interactionNPC;

    public Player Player
    {
        get => player;
    }

    public NPC InteractionNPC
    {
        get => interactionNPC;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        RespawnPlayer();

        interactionNPC = null;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;   // 마우스 커서를 화면 중앙에 고정하며, 마우스가 화면에서 보이지 않음
        Camera.main.AddComponent<CameraController>();
    }

    private void Update()
    {
        if (UIManager.Instance.IsUIOpen())
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void RespawnPlayer()
    {
        GameObject playerResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.Character, "Player");
        player = Instantiate(playerResource).AddComponent<Player>();
        string curScene = SceneManager.GetActiveScene().name;
        Vector3 respawnPos = Vector3.zero;
        switch (curScene)
        {
            case "Base Camp Scene":
                respawnPos = new Vector3(0, 0, 0);
                break;
            case "Field Scene":
                respawnPos = new Vector3(0, 0, 0);
                break;
            case "Boss Scene":
                respawnPos = new Vector3(0, 0, 0);
                break;
        }
        //player.transform.position = RayHelper.GetGroundPos(respawnPos).Value;
        player.transform.position = respawnPos;
    }

    public void AddInteractionNPC(NPC npc)
    {
        if (interactionNPC == null)
        {
            interactionNPC = npc;
        }
    }

    public void RemoveInteractionNPC(NPC npc)
    {
        if (interactionNPC == npc)
        {
            interactionNPC = null;
        }
    }
}
