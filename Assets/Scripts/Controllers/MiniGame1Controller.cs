using UnityEngine;
using Zenject;

public class MiniGame1Controller : MonoBehaviour
{
    [Inject] private ScenesManager scenes;

    private void Start()
    {
        scenes.FadeIn();
    }

    public void ToCord()
    {
        scenes.UnloadScene("MiniGame1");
    }
}
