using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayHelper
{
    /// <summary>
    /// 특정 위치에서 Y좌표 구하기
    /// </summary>
    /// <param name="origin">특정 위치</param>
    /// <returns>특정 위치에서 위에서 아래로 쏜 Ray에 맞은 Y좌표 값</returns>
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
