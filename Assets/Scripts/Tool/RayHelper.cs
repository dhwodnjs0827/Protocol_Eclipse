using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHelper
{
    /// <summary>
    /// Ư�� ��ġ���� Y��ǥ ���ϱ�
    /// </summary>
    /// <param name="origin">Ư�� ��ġ</param>
    /// <returns>Ư�� ��ġ���� ������ �Ʒ��� �� Ray�� ���� Y��ǥ ��</returns>
    public static Vector3? GetYPos(Vector3 origin)
    {
        Vector3 tmpPos = origin + new Vector3(0f, 200f, 0f);
        if (Physics.Raycast(tmpPos, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity))
        {
            return hitInfo.point;
        }
        else
        {
            return null;
        }
    }

    public static Vector3? GetGroundPos(Vector3 origin)
    {
        Vector3 tmpPos = origin + new Vector3(0f, 200f, 0f);
        int layerMask = 1 << LayerMask.NameToLayer("Ground");
        if (Physics.Raycast(tmpPos, Vector3.down, out RaycastHit hitInfo, Mathf.Infinity, layerMask))
        {
            return hitInfo.point;
        }
        else
        {
            return null;
        }
    }
}
