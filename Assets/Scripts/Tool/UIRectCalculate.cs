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
        // uiPos(이벤트 발생한 위치)와 자신의 영역 비교
        // 위치가 영역에 포함되어 있다면 true
        // 위치가 영역에 포함되어 있지 않다면 false
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
