using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �̱��� ��ü�� ����� ���� �̱��� Ŭ����
public class Singleton<T> where T : class, new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
}
