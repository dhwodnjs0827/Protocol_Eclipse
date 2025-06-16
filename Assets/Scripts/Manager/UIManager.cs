using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private UIFader fadeUI;
    [SerializeField] private PlayerUI playerUI;
    [SerializeField] private Canvas informationUI;
    [SerializeField] private NPCUI npcUI;

    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Button skillBtn;
    [SerializeField] private Button journalBtn;
    [SerializeField] private Button escapeBtn;
    [SerializeField] private Inventory inventory;

    private eUIState curUI;

    public static UIManager Instance
    {
        get => instance;
    }

    public PlayerUI PlayerUI
    {
        get => playerUI;
    }

    public NPCUI NPCUI
    {
        get => npcUI;
        set => npcUI = value;
    }

    private void Awake()
    {
        instance = this;

        curUI = eUIState.None;
    }

    private void Start()
    {
        informationUI.gameObject.SetActive(false);
        npcUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleSkill();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleJournal();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (curUI != eUIState.None)
            {
                CloseInformationUI();
            }
            else
            {
                // Menu 열기 코드 작성
            }
        }
    }

    private void OpenInformationUI(eUIState ui)
    {
        if (curUI == eUIState.None)
        {
            playerUI.gameObject.SetActive(false);
            informationUI.gameObject.SetActive(true);
        }
        switch (ui)
        {
            case eUIState.Inventory:
                inventoryBtn.interactable = false;
                skillBtn.interactable = true;
                journalBtn.interactable = true;
                inventory.gameObject.SetActive(true);
                break;
            case eUIState.Skill:
                inventoryBtn.interactable = true;
                skillBtn.interactable = false;
                journalBtn.interactable = true;
                inventory.gameObject.SetActive(false);
                break;
            case eUIState.Journal:
                inventoryBtn.interactable = true;
                skillBtn.interactable = true;
                journalBtn.interactable = false;
                inventory.gameObject.SetActive(false);
                break;
        }
        curUI = ui;
    }

    public void CloseInformationUI()
    {
        inventory.gameObject.SetActive(false);
        informationUI.gameObject.SetActive(false);

        inventoryBtn.interactable = true;
        skillBtn.interactable = true;
        journalBtn.interactable = true;

        playerUI.gameObject.SetActive(true);
        curUI = eUIState.None;
    }

    public void ToggleInventory()
    {
        if (curUI == eUIState.Inventory)
        {
            CloseInformationUI();
        }
        else
        {
            inventory.SetEquipmentSlot();
            inventory.SetWeaponList();
            OpenInformationUI(eUIState.Inventory);
        }
    }

    public void ToggleSkill()
    {
        if (curUI == eUIState.Skill)
        {
            CloseInformationUI();
        }
        else
        {
            OpenInformationUI(eUIState.Skill);
        }
    }

    public void ToggleJournal()
    {
        if (curUI == eUIState.Journal)
        {
            CloseInformationUI();
        }
        else
        {
            OpenInformationUI(eUIState.Journal);
        }
    }

    public bool IsUIOpen()
    {
        if (informationUI.gameObject.activeSelf || npcUI.gameObject.activeSelf)
        {
            return true;
        }
        return false;
    }

    public void OpenNPCUI()
    {
        playerUI.gameObject.SetActive(false);
        npcUI.gameObject.SetActive(true);
        int layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;
        Camera.main.cullingMask = layerMask;
    }

    public void CloseNPCUI()
    {
        playerUI.gameObject.SetActive(true);
        npcUI.gameObject.SetActive(false);
        Camera.main.cullingMask = -1;
    }

    public void FadeIn(float fadeTime)
    {
        fadeUI.FadeIn(fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        fadeUI.FadeOut(fadeTime);
    }

    public void FadeOutIn(float fadeTime)
    {
        fadeUI.FadeOutIn(fadeTime);
    }
}
