using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup fader;
    private bool isFadeInEnd;
    private bool isFadeOutEnd;

    private void Awake()
    {
        fader.alpha = 0f;
        isFadeInEnd = true;
        isFadeOutEnd = true;
    }

    public void FadeIn(float fadeTime)
    {
        if (isFadeInEnd == false)
        {
            return;
        }
        isFadeInEnd = false;
        StartCoroutine(FadeHelper.Fade(fader, 1f, 0f, fadeTime, () => isFadeInEnd = true));
    }

    public void FadeOut(float fadeTime)
    {
        if (isFadeOutEnd == false)
        {
            return;
        }
        isFadeOutEnd = false;
        StartCoroutine(FadeHelper.Fade(fader, 0f, 1f, fadeTime, () => isFadeOutEnd = true));
    }

    public void FadeOutIn(float fadeTime)
    {
        StartCoroutine(FadeHelper.FadeSequence(fader, fadeTime));
    }
}
