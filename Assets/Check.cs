using UnityEngine;
using Zenject;

public class Check : MonoBehaviour
{
    [Inject] private GameManager game;
    [SerializeField] GameObject go;

    private void Update()
    {
        go.SetActive(!game.onMini);
    }
}
