using UnityEngine;

public class Follow_Z : MonoBehaviour
{
    [SerializeField] public Transform zToFollow;
    [SerializeField] public float zoffset;

    private void Update()
    {
        transform.position = new(transform.position.x, transform.position.y, zToFollow.position.z + zoffset);
    }
}
