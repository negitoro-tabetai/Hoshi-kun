using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeManager : SingletonMonoBehaviour<FadeManager>
{
    [SerializeField, Tooltip("フェード用のイメージ")] Image _fadeImage;
    
    new void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    /// <summary>
    /// フェードイン -> シーン遷移 -> フェードアウト
    /// </summary>
    /// <param name="sceneName">遷移するシーン</param>
    /// <param name="fadeTime">フェードイン/アウトにかける時間</param>
    public void SceneFade(string sceneName, float fadeTime)
    {
        StartCoroutine(SceneFadeCoroutine(sceneName, fadeTime));
    }

    IEnumerator SceneFadeCoroutine(string sceneName, float fadeTime)
    {
        yield return FadeIn(fadeTime);
        SceneManager.LoadScene(sceneName);
        yield return FadeOut(fadeTime);
    }

    public IEnumerator FadeOut(float fadeTime)
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            Color color = _fadeImage.color;
            color.a = 1 - t / fadeTime;
            _fadeImage.color = color;
            yield return null;
        }
    }
    public IEnumerator FadeIn(float fadeTime)
    {
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            Color color = _fadeImage.color;
            color.a = t / fadeTime;
            _fadeImage.color = color;
            yield return null;
        }
    }
}
