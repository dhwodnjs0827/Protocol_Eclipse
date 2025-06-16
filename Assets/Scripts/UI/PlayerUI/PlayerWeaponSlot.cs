using DataDefinition;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSlot : MonoBehaviour
{
    [SerializeField] private Image weaponImage;
    [SerializeField] private TextMeshProUGUI weaponName;

    private Vector3 startPos;
    private Vector3 endPos;

    private bool isHighlight;
    private bool isMove;

    public bool IsHighlight
    {
        get => isHighlight;
    }
    public bool IsMove
    {
        get => isMove;
    }

    private void Awake()
    {
        weaponImage.sprite = null;
        weaponImage.color = Color.clear;
        weaponName.text = string.Empty;

        startPos = gameObject.transform.position;
        endPos = gameObject.transform.position + Vector3.right * 50f;

        isHighlight = false;
    }

    public void SetWeapon(Weapon weapon)
    {
        if (weapon != null)
        {
            weaponImage.sprite = weapon.ItemImage2x1;
            weaponImage.color = Color.white;
            weaponName.text = weapon.WeaponData.name;
        }
        else
        {
            weaponImage.sprite = null;
            weaponImage.color = Color.clear;
            weaponName.text = string.Empty;
        }
    }

    public IEnumerator HighlightSlot()
    {
        float elapesd = 0f;
        float duration = 0.3f;

        while (elapesd < duration)
        {
            isMove = true;
            gameObject.transform.position = Vector3.Lerp(startPos, endPos, elapesd / duration);
            elapesd += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = endPos;

        isHighlight = true;
        isMove = false;
    }

    public IEnumerator UnderStateSlot()
    {
        float elapesd = 0f;
        float duration = 0.3f;

        while (elapesd < duration)
        {
            isMove = true;
            gameObject.transform.position = Vector3.Lerp(endPos, startPos, elapesd / duration);
            elapesd += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = startPos;

        isHighlight = false;
        isMove = false;
    }
}
