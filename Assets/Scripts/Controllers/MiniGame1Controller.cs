using UnityEngine;
using Zenject;

public class MiniGame1Controller : MonoBehaviour
{
    [Inject] private ScenesManager scenes;
    [Inject] private GameManager game;

    [SerializeField] private float extraTime;
    [SerializeField] private float restTime;

    private void Start()
    {
        scenes.FadeIn();
    }

    public void ToCord(bool win)
    {
        if (win) 
        {
            game.time += extraTime;
        }
        else
        {
            game.time = restTime;
        }

        scenes.UnloadScene("MiniGame1");
        game.onMini = false;
    }
}
