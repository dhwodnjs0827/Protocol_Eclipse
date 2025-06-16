using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using Unity.VisualScripting;

public class ObjectPoolManager : MonoBehaviour
{
    private static ObjectPoolManager instance;

    private Dictionary<int, GameObject> weaponPoolDict;
    private Dictionary<eBulletImpactFX, List<GameObject>> bulletImpactFXPoolDict;
    private List<GameObject> muzzleFXPoolList;

    [SerializeField] private Transform weaponPool;
    [SerializeField] private Transform bulletImpactFXPool;
    [SerializeField] private Transform muzzleFXPool;

    public static ObjectPoolManager Instance
    {
        get => instance;
    }

    public Transform WeaponPool
    {
        get => weaponPool;
    }

    private void Awake()
    {
        instance = this;

        weaponPoolDict = new Dictionary<int, GameObject>();

        bulletImpactFXPoolDict = new Dictionary<eBulletImpactFX, List<GameObject>>();

        muzzleFXPoolList = new List<GameObject>();
    }

    public GameObject GetWeaponObject(int weaponID)
    {
        GameObject equippedWeapon = GetWeaponObjectInPool(weaponID);
        if (equippedWeapon != null)
        {
            equippedWeapon.SetActive(true);
        }
        else
        {
            Weapon[] equippedWeaponList = ItemManager.Instance.GetEquippedWeaponList();
            foreach (Weapon weapon in equippedWeaponList)
            {
                if (weapon != null && weapon.WeaponData.id == weaponID)
                {
                    GameObject weaponResource = weapon.WeaponPrefab;
                    equippedWeapon = Instantiate(weaponResource);
                    equippedWeapon.transform.parent = weaponPool;
                    weaponPoolDict.Add(weaponID, equippedWeapon);
                }
            }
        }
        return equippedWeapon;
    }

    private GameObject GetWeaponObjectInPool(int weaponID)
    {
        GameObject weaponObject;
        weaponPoolDict.TryGetValue(weaponID, out weaponObject);
        return weaponObject;
    }

    public GameObject GetBulletImpactFX(eBulletImpactFX fx)
    {
        GameObject bulletFX = GetBulletImpactFXInPool(fx);
        if (bulletFX != null)
        {
            bulletFX.SetActive(true);
        }
        else
        {
            GameObject fxResource = null;
            switch (fx)
            {
                case eBulletImpactFX.Concreate:
                    fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Bullet Impact Concrete");
                    break;
                case eBulletImpactFX.Metal:
                    fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Bullet Impact Metal");
                    break;
                case eBulletImpactFX.Sand:
                    fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Bullet Impact Sand");
                    break;
                case eBulletImpactFX.SoftBody:
                    fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Bullet Impact SoftBody");
                    break;
                case eBulletImpactFX.Wood:
                    fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Bullet Impact Wood");
                    break;
            }
            bulletFX = Instantiate(fxResource);
            PooledFX tmp = bulletFX.AddComponent<PooledFX>();
            tmp.ObjectPool = bulletImpactFXPool;
            bulletFX.transform.parent = bulletImpactFXPool;
            if (bulletImpactFXPoolDict.TryGetValue(fx, out List<GameObject> list))
            {
                bulletImpactFXPoolDict[fx].Add(bulletFX);
            }
            else
            {
                list = new List<GameObject>();
                list.Add(bulletFX);
                bulletImpactFXPoolDict.Add(fx, list);
            }
        }
        return bulletFX;
    }

    private GameObject GetBulletImpactFXInPool(eBulletImpactFX fx)
    {
        if (bulletImpactFXPoolDict.TryGetValue(fx, out List<GameObject> list))
        {
            GameObject bulletFX = list.Find(o => o.gameObject.activeSelf == false);
            return bulletFX;
        }
        return null;
    }

    public GameObject GetMuzzleFX()
    {
        GameObject muzzleFX = GetMuzzleFXInPool();
        if (muzzleFX != null)
        {
            muzzleFX.SetActive(true);
        }
        else
        {
            GameObject fxResource = ResourceManager.Instance.GetPrefabResource(ePrefabType.FX, "Muzzle FX");
            muzzleFX = Instantiate(fxResource);
            PooledFX tmp = muzzleFX.AddComponent<PooledFX>();
            tmp.ObjectPool = muzzleFXPool;
            muzzleFX.transform.parent = muzzleFXPool;
            if (muzzleFXPoolList != null)
            {
                muzzleFXPoolList.Add(muzzleFX);
            }
            else
            {
                muzzleFXPoolList = new List<GameObject>();
                muzzleFXPoolList.Add(muzzleFX);
            }
        }
        return muzzleFX;
    }

    private GameObject GetMuzzleFXInPool()
    {
        GameObject muzzleFX = muzzleFXPoolList.Find(o => o.activeSelf == false);
        return muzzleFX;
    }
}
