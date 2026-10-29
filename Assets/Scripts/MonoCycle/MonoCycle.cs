using PrimeTween;
using UnityEngine;

public class MonoCycle : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private bool move;
    [SerializeField] private float speed;

    [SerializeField] private bool rotate;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Ease rotationEase;
    [SerializeField] private Transform toRotate;

    private void Start()
    {
        TryGetComponent(out rb);

        if (!rotate) { return; }
        Tween.EulerAngles(toRotate, transform.eulerAngles, new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -360), rotationSpeed, rotationEase, -1, CycleMode.Incremental);
    }

    private void Update()
    {

        //toRotate.localEulerAngles = new(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + rotationSpeed);
    }

    private void FixedUpdate()
    {
        if (!move) { return; }

        rb.linearVelocity = new(0, rb.linearVelocity.y, speed);
    }
}
