using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeHelper
{
    public static IEnumerator Fade(CanvasGroup fader, float startAlpha, float endAlpha, float fadeTime, System.Action onComplete = null)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fader.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeTime);
            yield return null;
        }
        fader.alpha = endAlpha;
        onComplete?.Invoke();
    }

    public static IEnumerator FadeSequence(CanvasGroup fader, float fadeTime)
    {
        yield return Fade(fader, 0f, 1f, fadeTime * 0.5f);
        yield return Fade(fader, 1f, 0f, fadeTime * 0.5f);
    }
}
