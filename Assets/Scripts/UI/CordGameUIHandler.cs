using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

public class CordGameUIHandler : MonoBehaviour
{
    [SerializeField] private Image current;
    [SerializeField] private Image HudScreen, pauseScreen, endScreen;

    [SerializeField] private float appearigTime;
    [SerializeField] private Ease appearigEase;

    public void ChangeScreen(Image nextScreen)
    {
        Tween.Scale(current.transform, 0, appearigTime, appearigEase).OnComplete(() =>
        {
            current.gameObject.SetActive(false);
            nextScreen.gameObject.SetActive(true);
            Tween.Scale(nextScreen.transform, 1, appearigTime, appearigEase);
            current = nextScreen;
        });
    }
}
