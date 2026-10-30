using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Checker : MonoBehaviour
{
    [SerializeField] PlayerCordMovement cord;
    int times = 0;

    private void OnEnable()
    {
        if (times == 0) 
        {
            times++;
            return; 
        }
        times++;
        cord.Restart();
    }
}
