using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

delegate void setCurWeaponUI();

[Serializable]
struct PlayerWeaponInfo
{
    public Weapon weapon;
    public PlayerWeaponSlot slot;
}

public class PlayerUI : MonoBehaviour, EquippedWeaponSlotObserver, CurrentEquippedWeaponObserver
{
    [SerializeField] private PlayerWeaponInfo[] playerWeaponData;

    private setCurWeaponUI highlightSlot;

    [SerializeField] Image interactionTextBackground;
    [SerializeField] private Slider shieldBar;
    [SerializeField] private Slider hpBar;
    [SerializeField] private Image crosshair;
    [SerializeField] private TextMeshProUGUI curMagazine;
    [SerializeField] private TextMeshProUGUI totalBullet;
    [SerializeField] private QuestWindow questWindow;

    private int curSlotIndex;
    private Sprite defaultCrosshair;

    private void Awake()
    {
        interactionTextBackground.gameObject.SetActive(false);

        highlightSlot = null;
        curMagazine.text = string.Empty;
        totalBullet.text = string.Empty;

        curSlotIndex = 0;
        defaultCrosshair = ResourceManager.Instance.GetSpriteResource(eSpriteType.UI, "Default Crosshair");
    }

    private void Start()
    {
        crosshair.sprite = defaultCrosshair;
        ItemManager.Instance.AddObserver(this);
        PlayManager.instance.Player.AddCurrentEquippedWeaponObserver(this);
    }

    private void Update()
    {
        if (highlightSlot != null)
        {
            highlightSlot();
            highlightSlot = null;
        }
    }

    void UpdateCurrentWeaponUI()
    {
        if (playerWeaponData[curSlotIndex].weapon == null && playerWeaponData[curSlotIndex].slot.IsHighlight)
        {
            playerWeaponData[curSlotIndex].slot.StartCoroutine(playerWeaponData[curSlotIndex].slot.UnderStateSlot());
            crosshair.sprite = defaultCrosshair;
            curMagazine.text = string.Empty;
            totalBullet.text = string.Empty;
        }
        else if (playerWeaponData[curSlotIndex].weapon != null && !playerWeaponData[curSlotIndex].slot.IsHighlight)
        {
            playerWeaponData[curSlotIndex].slot.StartCoroutine(playerWeaponData[curSlotIndex].slot.HighlightSlot());
            crosshair.sprite = playerWeaponData[curSlotIndex].weapon.AimImage;
            curMagazine.text = playerWeaponData[curSlotIndex].weapon.CurMagazine.ToString();
            totalBullet.text = " / " + (playerWeaponData[curSlotIndex].weapon.TotalBullet - playerWeaponData[curSlotIndex].weapon.CurMagazine);
        }
    }

    public void ChangeEquippedWeaponList(Weapon[] equippedWeapon)
    {
        for (int i = 0; i < playerWeaponData.Length; i++)
        {
            if (equippedWeapon[i] != null)
            {
                playerWeaponData[i].weapon = equippedWeapon[i];
                playerWeaponData[i].slot.SetWeapon(playerWeaponData[i].weapon);
            }
            else
            {
                playerWeaponData[i].weapon = null;
                playerWeaponData[i].slot.SetWeapon(playerWeaponData[i].weapon);
            }
        }
        highlightSlot = UpdateCurrentWeaponUI;
    }

    public void UseBullet(int equippedWeaponIndex)
    {
        curMagazine.text = playerWeaponData[equippedWeaponIndex].weapon.CurMagazine.ToString();
    }

    public void ReloadWeapon(int equippedWeaponIndex)
    {
        curMagazine.text = playerWeaponData[equippedWeaponIndex].weapon.CurMagazine.ToString();
        totalBullet.text = " / " + (playerWeaponData[equippedWeaponIndex].weapon.TotalBullet - playerWeaponData[equippedWeaponIndex].weapon.CurMagazine);
    }

    public void SwapCurWeapon(int equippedWeaponIndex)
    {
        if (!playerWeaponData[curSlotIndex].slot.IsMove)
        {
            if (playerWeaponData[curSlotIndex].slot.IsHighlight)
            {
                playerWeaponData[curSlotIndex].slot.StartCoroutine(playerWeaponData[curSlotIndex].slot.UnderStateSlot());
            }

            if (playerWeaponData[equippedWeaponIndex].weapon != null)
            {
                playerWeaponData[equippedWeaponIndex].slot.StartCoroutine(playerWeaponData[equippedWeaponIndex].slot.HighlightSlot());
                curMagazine.text = playerWeaponData[equippedWeaponIndex].weapon.CurMagazine.ToString();
                totalBullet.text = " / " + (playerWeaponData[equippedWeaponIndex].weapon.TotalBullet - playerWeaponData[equippedWeaponIndex].weapon.CurMagazine);
                crosshair.sprite = playerWeaponData[equippedWeaponIndex].weapon.AimImage;
            }
            else
            {
                crosshair.sprite = defaultCrosshair;
                curMagazine.text = string.Empty;
                totalBullet.text = string.Empty;
            }
            curSlotIndex = equippedWeaponIndex;
        }
    }

    public void ActiveInteractionUI(bool isTrigger)
    {
        interactionTextBackground.gameObject.SetActive(isTrigger);
    }
}
