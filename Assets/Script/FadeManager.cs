using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private float fadeDuration = 1.0f;
    public IEnumerator FadeOutAndLoad(SceneName sceneName)
    {
        yield return StartCoroutine(FadeOut());

        SceneManager.LoadScene(sceneName.ToString());
    }

    /// <summary>
    /// 画面が明るくなる方
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeIn()
    {
        fadeImage.enabled = true;
        yield return StartCoroutine(Fade(1.0f, 0.0f));
        fadeImage.enabled = false;
    }

    /// <summary>
    /// 画面が暗くなる方
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOut()
    {
        fadeImage.enabled = true;
        yield return StartCoroutine(Fade(0.0f, 1.0f));
    }
    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(
                startAlpha,
                endAlpha,
                time / fadeDuration
            );

            SetAlpha(alpha);

            yield return null;
        }

        // 最終値を保証
        SetAlpha(endAlpha);
    }
    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}