using System;
using UnityEngine;
using Zenject;

public class CordGameController : MonoBehaviour
{
    [Inject] private GameManager game;
    [Inject] private ScenesManager scenes;
    [Inject] private TimerManager timer;

    [SerializeField] private HudUIHandler hud;
    [SerializeField] private Transform fox;

    [SerializeField] private float playingTime;

    
    int mins, scs;

    private void Start()
    {
        scenes.FadeIn();
        timer.CreateTimer("Cord", TimerType.Countdown, TimerEvent.Update, UpdateTimer);
        timer.StartCountdown("Cord", playingTime);
    }

    private void Update()
    {
        int pos = (int) fox.position.z;
        hud.UpdateDistance(pos.ToString());

        game.time -= Time.deltaTime;
        mins = (int)(game.time / 60);
        scs = (int)(game.time - mins * 60f);
    }

    private void UpdateTimer()
    {
        string time = string.Format("{0:00}:{1:00}", mins, scs);
        hud.UpdateTime(time);
    }

    public void ToMiniGame()
    {
        scenes.ChangeSceneAdditive("MiniGame1");
    }
}
