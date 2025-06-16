using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackAction : MonoBehaviour
{
    private Weapon curWeapon;
    private Transform curWeaponMuzzlePos;

    private Rigidbody rb;
    private Animator animator;
    private Camera mainCam;

    public void Initailize(Rigidbody rigidbody, Animator animator, Camera camera)
    {
        rb = rigidbody;
        this.animator = animator;
        mainCam = camera;
    }

    public void FireGun()
    {
        Debug.Log("น฿ป็");
        animator.SetTrigger("fire");
    }

    private void MuzzleFX()
    {
        GameObject muzzleFX = ObjectPoolManager.Instance.GetMuzzleFX();
        muzzleFX.transform.SetParent(curWeaponMuzzlePos);
        muzzleFX.transform.localPosition = Vector3.zero;
        muzzleFX.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    private void Hitscan()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        int layerMask = 1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Area");
        layerMask = ~layerMask;
        if (Physics.Raycast(ray, out RaycastHit bulletHitInfo, 30f, layerMask))
        {
            GameObject bulletFX = null;
            switch (bulletHitInfo.collider.tag)
            {
                case "Ground":
                    bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Concreate);
                    break;
                case "Fence":
                    bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Metal);
                    break;
                case "Monster":
                    bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.SoftBody);
                    bulletHitInfo.collider.SendMessage("Damaged", curWeapon.BulletPower);
                    break;
                case "Props":
                    bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Wood);
                    break;
            }
            bulletFX.transform.position = bulletHitInfo.point;
            bulletFX.transform.rotation = Quaternion.LookRotation(bulletHitInfo.normal);
        }
    }

    private void Projectile()
    {
        // ToDo
    }

    private void BulletImpactFX(GameObject targetObject, Vector3 fxPos)
    {
        GameObject bulletFX = null;
        switch (targetObject.tag)
        {
            case "Ground":
                bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Concreate);
                break;
            case "Fence":
                bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Metal);
                break;
            case "Monster":
                bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.SoftBody);
                targetObject.SendMessage("Damaged", curWeapon.BulletPower);
                break;
            case "Props":
                bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Wood);
                break;
        }
        bulletFX.transform.position = fxPos;
        //bulletFX.transform.rotation = Quaternion.LookRotation(bulletHitInfo.normal);
    }
}
