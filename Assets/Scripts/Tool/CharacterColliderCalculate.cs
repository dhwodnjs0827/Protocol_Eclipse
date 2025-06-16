using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColliderCalculate : MonoBehaviour
{
    public CapsuleCollider capsuleCollider;
    SkinnedMeshRenderer[] renderers;

    void Start()
    {
        // 캡슐 콜라이더와 캐릭터 렌더러를 가져옴
        capsuleCollider = GetComponent<CapsuleCollider>();
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // 자식에 있는 렌더러 가져오기

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

        // 캐릭터 높이를 계산
        float characterHeight = result;

        // 캡슐 콜라이더의 Height를 설정
        capsuleCollider.height = characterHeight;

        // 캡슐 콜라이더의 센터를 캐릭터 중간으로 설정
        capsuleCollider.center = new Vector3(0, characterHeight / 2, 0);
    }
}
