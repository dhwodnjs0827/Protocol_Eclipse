using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRectCalculate
{
    static public Rect RectInfo(RectTransform rectTr)
    {
        Rect rect = new Rect();
        rect.x = rectTr.position.x - rectTr.rect.width * 0.5f;
        rect.y = rectTr.position.y + rectTr.rect.height * 0.5f;
        rect.width = rectTr.rect.width;
        rect.height = rectTr.rect.height;
        return rect;
    }

    static public bool IsInRect(Rect rect, Vector2 uiMousePos)
    {
        // uiPos(�̺�Ʈ �߻��� ��ġ)�� �ڽ��� ���� ��
        // ��ġ�� ������ ���ԵǾ� �ִٸ� true
        // ��ġ�� ������ ���ԵǾ� ���� �ʴٸ� false
        if (uiMousePos.x >= rect.x 
         && uiMousePos.x <= rect.x + rect.width
         && uiMousePos.y <= rect.position.y
         && uiMousePos.y >= rect.position.y - rect.height)
        {
            return true;
        }
        return false;
    }
}
