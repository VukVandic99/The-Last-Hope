using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeVictoryScreen : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 5.0f;

    public void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(255f, 255f, 138f, 1f); // Black with alpha 1.

        while (timer < fadeDuration)
        {
            // Interpolate the color between start and end over time.
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = endColor;

    }
}