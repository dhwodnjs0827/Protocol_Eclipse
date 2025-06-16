using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ΩÃ±€≈Ê ∞¥√º∏¶ ∏∏µÈ±‚ ¿ß«— ΩÃ±€≈Ê ≈¨∑°Ω∫
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
