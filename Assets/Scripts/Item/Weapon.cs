using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    private WeaponData weaponData;

    private GameObject weaponPrefab;
    private Sprite itemImage2x1;
    private Sprite aimImage;

    private int maxMagazine;
    private int curMagazine;
    private float fireIntervalTime;
    private float reloadTime;

    public int TotalBullet
    {
        get => ItemManager.Instance.GetTotalBullet(weaponData.weaponType);
    }

    public WeaponData WeaponData
    {
        get => weaponData;
    }

    public GameObject WeaponPrefab
    {
        get => weaponPrefab;
    }

    public Sprite ItemImage2x1
    {
        get => itemImage2x1;
    }

    public Sprite AimImage
    {
        get => aimImage;
    }

    public int MaxMagazine
    {
        get => maxMagazine;
    }

    public int CurMagazine
    {
        get => curMagazine;
    }

    public float FireIntervalTime
    {
        get => fireIntervalTime;
    }

    public float ReloadTime
    {
        get => reloadTime;
    }

    public float WeaponPower
    {
        get => weaponData.attackPowerPerBullet * weaponData.rpm;
    }

    public float BulletPower
    {
        get => weaponData.attackPowerPerBullet;
    }

    public Weapon(int dataID) : base(dataID)
    {
        itemImage = ResourceManager.Instance.GetSpriteResource(eSpriteType.Weapon, itemData.spriteResourceName);

        weaponData = DataTableManager.Instance.InitializeDataStruct<WeaponData>(eDataTable.Weapon, dataID);

        weaponPrefab = ResourceManager.Instance.GetPrefabResource(ePrefabType.Weapon, weaponData.prefabResourceName);
        itemImage2x1 = ResourceManager.Instance.GetSpriteResource(eSpriteType.Weapon, weaponData.sprite2x1ResourceName);
        aimImage = ResourceManager.Instance.GetSpriteResource(eSpriteType.Weapon, weaponData.aimResourceName);

        maxMagazine = weaponData.magazine;
        fireIntervalTime = weaponData.fireIntervalTime;
        reloadTime = weaponData.reloadTime;
    }

    public void ShotCurMagazine()
    {
        curMagazine -= 1;
    }

    public void ReloadMagazine()
    {
        int needMagazine = maxMagazine - curMagazine;   // 필요한 탄 개수
        int bulletToReload = Mathf.Min(needMagazine, TotalBullet - curMagazine);
        curMagazine += bulletToReload;
    }
}
