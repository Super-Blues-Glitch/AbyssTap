using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI distance, time;
    [SerializeField] private Button pauseBT;
    [SerializeField] private CordGameUIHandler cordUI;
    [SerializeField] private Image pauseImage;

    private void Start()
    {
        //pauseBT.onClick.AddListener(() => cordUI.ChangeScreen(pauseImage, () => Debug.Log("Hola")));
    }

    public void UpdateDistance(string newDistance)
    {
        distance.text = newDistance + " m";
    }

    public void UpdateTime(string newTime)
    {
        time.text = newTime;
    }
}
