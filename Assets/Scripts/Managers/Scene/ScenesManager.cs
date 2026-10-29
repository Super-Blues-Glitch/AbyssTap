using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : MonoBehaviour
{
    [SerializeField] private Image fadeImg;
    [SerializeField] private float fadeDuration;
    Tween fadeIn, fadeOut;

    bool changing = false;


    public void ChangeScene(string sceneName)
    {
        if (changing) { return;  } 

        changing = true;
        FadeOut(() => SceneManager.LoadScene(sceneName));
    }
    

    public void ChangeSceneAdditive(string sceneName)
    {
        fadeImg.raycastTarget = true;
        FadeOut(() => StartCoroutine(ChangeAdditive(sceneName)));
    }

    public void UnloadScene(string sceneName)
    {
        fadeImg.raycastTarget = true;
        FadeOut(() => StartCoroutine(UnloadAdditive(sceneName)));
    }

    public IEnumerator ChangeAdditive(string sceneName)
    {
        AsyncOperation change = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return change;
    }

    public IEnumerator UnloadAdditive(string sceneName)
    {
        AsyncOperation change = SceneManager.UnloadSceneAsync(sceneName);
        yield return change;
        FadeIn();
    }

    public void FadeOut(Action func = null)
    {
        fadeImg.raycastTarget = true;
        fadeOut = Tween.Alpha(fadeImg, 1, fadeDuration).OnComplete(() =>
        {
            Time.timeScale = 1;
            func?.Invoke();
            fadeOut.Stop();
        });
    }

    public void FadeIn(Action func = null)
    {
        fadeIn = Tween.Alpha(fadeImg, 0, fadeDuration).OnComplete(() =>
        {
            fadeImg.raycastTarget = false;
            func?.Invoke();
            changing = false;
            fadeIn.Stop();
        }
        );
    }

    public string GetActiveScene() => SceneManager.GetActiveScene().name;
}
