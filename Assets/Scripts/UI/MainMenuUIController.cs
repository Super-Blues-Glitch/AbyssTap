using PrimeTween;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenuUIController : MonoBehaviour
{
    [Inject] private ScenesManager scenes;

    [Header("Screens")]
    [SerializeField] private Image current;
    [SerializeField] private Image mainScreen, storyScreen, instructionsScreen;
    [SerializeField] private float appearigTime;
    [SerializeField] private Ease appearigEase;

    [Header("Main")]
    [SerializeField] private Button playBT;
    [SerializeField] private Button exitBT;

    [Header("Play")]
    [SerializeField] private Button exitStoryBT;
    [SerializeField] private Button exitInstructionsBT;
    [SerializeField] private Button toInstructionsBT;
    [SerializeField] private Button toGameBT;

    [Header("Options")]
    [SerializeField] private Button exitOptionsBT;


    private void Start()
    {
        scenes.FadeIn();

        playBT.onClick.AddListener(() => ChangeScreen(storyScreen));
        exitBT.onClick.AddListener(Exit);

        exitStoryBT.onClick.AddListener(() => ChangeScreen(mainScreen));
        exitInstructionsBT.onClick.AddListener(() => ChangeScreen(mainScreen));

        toInstructionsBT.onClick.AddListener(() => ChangeScreen(instructionsScreen));
        toGameBT.onClick.AddListener(() => scenes.ChangeScene("Cord"));
    }

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

    private void Exit()
    {
        Application.Quit();
    }
}
