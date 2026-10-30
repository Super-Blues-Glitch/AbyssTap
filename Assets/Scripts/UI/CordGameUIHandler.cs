using PrimeTween;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class CordGameUIHandler : MonoBehaviour
{
    [Inject] private GameManager game;
    [Inject] private ScenesManager scenes;
    
    [SerializeField] private Image current;
    [SerializeField] private Image HudScreen, pauseScreen, endScreen;

    [SerializeField] private float appearigTime;
    [SerializeField] private Ease appearigEase;

    [SerializeField] private Button restart, exit;
    bool su;

    private void Start()
    {
        restart.onClick.AddListener(() =>
        {
            game.time = 60;
            scenes.ChangeScene("Cord");
            });
        exit.onClick.AddListener(() =>
        {
            scenes.ChangeScene("MainMenu");
        });
    }

    private void Update()
    {
        if (game.time <= 0 && !su)
        {
            su = true;
            ChangeScreen(endScreen, () => Debug.Log("s"));
        }
    }

    public void ChangeScreen(Image nextScreen, Action finish)
    {
        Tween.Scale(current.transform, 0, appearigTime, appearigEase).OnComplete(() =>
        {
            current.gameObject.SetActive(false);
            nextScreen.gameObject.SetActive(true);
            Tween.Scale(nextScreen.transform, 1, appearigTime, appearigEase);
            finish?.Invoke();
            current = nextScreen;
        });
    }
}
