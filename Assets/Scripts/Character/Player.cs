using DataDefinition;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, EquippedWeaponSlotObserver
{
    private PlayerStatusData playerStatus;

    List<CurrentEquippedWeaponObserver> curEquippedWeaponObserverList;
    InteractionObserver interactionObserver;

    private int maxExp;
    private int curExp;

    private float walkSpeed;
    private float runSpeed;
    private float moveSpeed;
    private bool isMove;
    private bool isRun;
    private Vector3 keyDir;

    private float rotateSpeed;
    private float rotateY;

    private int maxJumpCount;
    private int jumpCount;
    private float jumpPower;
    private bool isGrounded;

    private Weapon[] equippedWeaponList;
    private int curWeaponIndex;

    private Transform weaponPool;
    private Transform weaponPos;
    private GameObject curWeaponObject;
    private Transform curWeaponMuzzlePos;

    private float shootElapesdTime;
    private bool isReloading;
    private float reloadElapsedTime;
    private bool isAim;

    private Armor[] equippedArmor;

    private bool isInteractable;

    private Rigidbody rb;
    private Animator animator;

    public PlayerStatusData PlayerStatus
    {
        get => playerStatus;
    }

    public bool IsInteractable
    {
        set
        {
            if (isInteractable == value)
            {
                return;
            }
            isInteractable = value;
        }
    }

    private void Awake()
    {
        playerStatus = new PlayerStatusData();
        playerStatus.level = 1;
        playerStatus.maxHP = 100;
        playerStatus.maxShield = 100;

        curEquippedWeaponObserverList = new List<CurrentEquippedWeaponObserver>();
        interactionObserver = null;

        walkSpeed = 5f;
        runSpeed = 10f;
        rotateSpeed = 50f;
        jumpPower = 4f;
        maxJumpCount = 2;

        moveSpeed = 0f;
        isMove = false;
        isRun = false;

        rotateY = 0f;

        keyDir = Vector3.zero;

        jumpCount = 0;
        isGrounded = true;

        isAim = false;

        weaponPos = gameObject.transform.Find("root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/WeaponPos");
        curWeaponObject = null;
        curWeaponIndex = 0;
        equippedWeaponList = new Weapon[3];
        equippedArmor = new Armor[3];
        shootElapesdTime = 0f;
        isReloading = false;
        reloadElapsedTime = 0f;

        isInteractable = false;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        weaponPool = ObjectPoolManager.Instance.WeaponPool;
        ItemManager.Instance.AddObserver(this);
    }

    private void FixedUpdate()
    {
        if (!UIManager.Instance.IsUIOpen())
        {
            MovementFixedUpdate();
        }
    }

    private void Update()
    {
        if (!UIManager.Instance.IsUIOpen())
        {
            shootElapesdTime += Time.deltaTime;
            if (isReloading)
            {
                reloadElapsedTime += Time.deltaTime;
                if (reloadElapsedTime >= equippedWeaponList[curWeaponIndex].WeaponData.reloadTime)
                {
                    NotifyReloadToCurrentEquippedWeaponObserver(curWeaponIndex);
                    isReloading = false;
                    reloadElapsedTime = 0f;
                }
            }

            MovementUpdate();

            CurWeaponUpdate();

            if (isInteractable && Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("EŰ ����");
                if (interactionObserver != null)
                {
                    Debug.Log("��ȣ�ۿ� ������ ����");
                    NotifyInteractionToInteractionObserver();
                }
            }
        }
        else
        {
            animator.Rebind();
        }
    }

    private void LateUpdate()
    {
        if (isAim)
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 20f, 0.5f);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60f, 0.5f);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            jumpCount = 0;
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")))
        {
            isGrounded = false;
        }
    }

    private void MovementFixedUpdate()
    {
        // �̵�
        Vector3 worldKeyDir = transform.TransformDirection(keyDir);    // keyDir�� �÷��̾� ���� ��ǥ�迡�� ���� ��ǥ�� �������� ��ȯ
        Vector3 tmpVelocity = new Vector3(worldKeyDir.x * moveSpeed, rb.velocity.y, worldKeyDir.z * moveSpeed);
        rb.velocity = tmpVelocity;

        // ȸ��
        Quaternion rotate = Quaternion.Euler(0, rotateY * rotateSpeed * Time.fixedDeltaTime, 0);
        rb.MoveRotation(rb.rotation * rotate);

        // ����
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumpCount)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            jumpCount++;
        }
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * 9.8f, ForceMode.Acceleration);
        }
    }

    private void MovementUpdate()
    {
        // ȸ��
        rotateY = Input.GetAxis("Mouse X");

        // �̵�
        float horizontal = Input.GetAxis("Horizontal"); // ���� ����
        float vertical = Input.GetAxis("Vertical"); // ���� ����
        keyDir = new Vector3(horizontal, 0, vertical).normalized;  // �̵� ���� ����ȭ
        isMove = keyDir.magnitude > 0;    // Ű �Է� ������ �̵� ���� true
        isRun = Input.GetKey(KeyCode.LeftShift);
        moveSpeed = isRun ? runSpeed : walkSpeed; // LShift ������ ������ �޸��� �ӵ� ���� / �ƴϸ� �ȱ� �ӵ� ����

        animator.SetFloat("vertical", vertical);                // �յ�
        animator.SetFloat("horizontal", horizontal);            // �¿�
        animator.SetBool("isMove", isMove);                     // �̵�
        animator.SetBool("isRun", isRun);                       // �޸���
    }

    private void CurWeaponUpdate()
    {
        // ���� ��ü
        if (Input.GetKeyDown(KeyCode.Alpha1) && curWeaponIndex != 0)
        {
            curWeaponIndex = 0;
            NotifySwapWeaponToCurrentEquippedWeaponObserver(curWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && curWeaponIndex != 1)
        {
            curWeaponIndex = 1;
            NotifySwapWeaponToCurrentEquippedWeaponObserver(curWeaponIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && curWeaponIndex != 2)
        {
            curWeaponIndex = 2;
            NotifySwapWeaponToCurrentEquippedWeaponObserver(curWeaponIndex);
        }

        if (equippedWeaponList[curWeaponIndex] != null)
        {
            // ������
            if (Input.GetKeyDown(KeyCode.R) && equippedWeaponList[curWeaponIndex].CurMagazine != equippedWeaponList[curWeaponIndex].MaxMagazine && isReloading == false)
            {
                ReloadWeapon();
            }

            // �߻�
            if (Input.GetMouseButton(0) && equippedWeaponList[curWeaponIndex].CurMagazine > 0 && shootElapesdTime > equippedWeaponList[curWeaponIndex].FireIntervalTime && isReloading == false)
            {
                ShootWeapon();
                shootElapesdTime = 0f;
            }

            // ����
            if (Input.GetMouseButton(1))
            {
                isAim = true;
                if (isGrounded)
                {
                    isRun = false;
                    moveSpeed = walkSpeed;
                }
            }
            else
            {
                isAim = false;
            }
            animator.SetBool("isAim", isAim);     // ����
        }
    }

    private void ReloadWeapon()
    {
        animator.SetFloat("reloadTime", 3.3f / equippedWeaponList[curWeaponIndex].ReloadTime);
        animator.SetTrigger("reload");
        isReloading = true;
    }

    private void ShootWeapon()
    {
        NotifyShotToCurrentEquippedWeaponObserver(curWeaponIndex);
        animator.SetTrigger("fire");
        shootElapesdTime = Time.time + equippedWeaponList[curWeaponIndex].FireIntervalTime;

        // ����Ʈ
        GameObject muzzleFX = ObjectPoolManager.Instance.GetMuzzleFX();
        muzzleFX.transform.SetParent(curWeaponMuzzlePos);
        muzzleFX.transform.localPosition = Vector3.zero;
        muzzleFX.transform.localEulerAngles = new Vector3(0, 180, 0);

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
                    bulletHitInfo.collider.SendMessage("Damaged", equippedWeaponList[curWeaponIndex].BulletPower);
                    break;
                case "Props":
                    bulletFX = ObjectPoolManager.Instance.GetBulletImpactFX(eBulletImpactFX.Wood);
                    break;
            }
            bulletFX.transform.position = bulletHitInfo.point;
            bulletFX.transform.rotation = Quaternion.LookRotation(bulletHitInfo.normal);
        }
    }

    public void UpdateArmorData(Armor[] equippedArmorList)
    {
        for (int i = 0; i < equippedArmorList.Length; i++)
        {
            if (equippedArmorList[i] != null)
            {
                equippedArmor[i] = equippedArmorList[i];
            }
            else
            {
                equippedArmor[i] = null;
            }
        }
    }

    private void SetCurWeaponObject()
    {
        if (curWeaponObject != null)
        {
            curWeaponObject.transform.parent = weaponPool;
            curWeaponObject.SetActive(false);
        }
        if (equippedWeaponList[curWeaponIndex] != null)
        {
            curWeaponObject = ObjectPoolManager.Instance.GetWeaponObject(equippedWeaponList[curWeaponIndex].ItemID);
            curWeaponObject.transform.parent = weaponPos;
            curWeaponObject.transform.position = weaponPos.position;
            curWeaponObject.transform.rotation = weaponPos.rotation;

            curWeaponMuzzlePos = curWeaponObject.transform.Find("MuzzlePos");

            playerStatus.attackPower = equippedWeaponList[curWeaponIndex].WeaponPower;
            playerStatus.fireAttack = equippedWeaponList[curWeaponIndex].WeaponData.firePower;
            playerStatus.iceAttack = equippedWeaponList[curWeaponIndex].WeaponData.icePower;
            playerStatus.electricityAttack = equippedWeaponList[curWeaponIndex].WeaponData.electricityPower;
        }
    }

    public void ChangeEquippedWeaponList(Weapon[] weaponList)
    {
        for (int i = 0; i < weaponList.Length; i++)
        {
            if (weaponList[i] != null)
            {
                equippedWeaponList[i] = weaponList[i];
            }
            else
            {
                equippedWeaponList[i] = null;
            }
        }

        SetCurWeaponObject();
    }

    public void AddCurrentEquippedWeaponObserver(CurrentEquippedWeaponObserver observer)
    {
        if (!curEquippedWeaponObserverList.Contains(observer))
        {
            curEquippedWeaponObserverList.Add(observer);
        }
    }

    public void NotifyShotToCurrentEquippedWeaponObserver(int equippedWeaponIndex)
    {
        foreach (CurrentEquippedWeaponObserver observer in curEquippedWeaponObserverList)
        {
            observer.UseBullet(equippedWeaponIndex);
        }
    }

    public void NotifyReloadToCurrentEquippedWeaponObserver(int equippedWeaponIndex)
    {
        foreach (CurrentEquippedWeaponObserver observer in curEquippedWeaponObserverList)
        {
            observer.ReloadWeapon(equippedWeaponIndex);
        }
    }

    public void NotifySwapWeaponToCurrentEquippedWeaponObserver(int equippedWeaponIndex)
    {
        SetCurWeaponObject();
        foreach (CurrentEquippedWeaponObserver observer in curEquippedWeaponObserverList)
        {
            observer.SwapCurWeapon(equippedWeaponIndex);
        }
    }

    public void AddInteractionObserver(InteractionObserver observer)
    {
        if (interactionObserver == null)
        {
            interactionObserver = observer;
        }
    }

    public void RemoveInteractionObserver(InteractionObserver observer)
    {
        if (interactionObserver == observer)
        {
            interactionObserver = null;
        }
    }

    public void NotifyInteractionToInteractionObserver()
    {
        interactionObserver.ActiveInteraction();
    }
}
