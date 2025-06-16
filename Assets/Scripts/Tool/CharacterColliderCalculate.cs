using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderCalculate : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;
    SkinnedMeshRenderer[] renderers;

    void Start()
    {
        // ĸ�� �ݶ��̴��� ĳ���� �������� ������
        capsuleCollider = GetComponent<CapsuleCollider>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // �ڽĿ� �ִ� ������ ��������

        AdjustCapsuleCollider();
    }

    void AdjustCapsuleCollider()
    {
        float tmp1 = renderers[0].bounds.size.y;
        float tmp2;
        float result = 0f;
        foreach (var renderer in renderers)
        {
            tmp2 = renderer.bounds.size.y;
            if (tmp2 > tmp1)
            {
                result = tmp2;
                tmp1 = tmp2;
            }
            else
            {
                result = tmp1;
            }
        }

        // ĳ���� ���̸� ���
        float characterHeight = result;

        // ĸ�� �ݶ��̴��� Height�� ����
        capsuleCollider.height = characterHeight;

        // ĸ�� �ݶ��̴��� ���͸� ĳ���� �߰����� ����
        capsuleCollider.center = new Vector3(0, characterHeight / 2, 0);
    }
}
