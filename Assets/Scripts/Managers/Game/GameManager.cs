using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float time;

    private void Start()
    {
        GameEvents.Game.GamePaused += GamePaused; 
        GameEvents.Game.GameResumed += GameResumed; 
    }

    private void GamePaused()
    {
        Time.timeScale = 0;
    }

    private void GameResumed()
    {
        Time.timeScale = 1;
    }
}
