using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Inject] InputsManager inputs;
    Rigidbody rb;

    private Vector2 movement;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Ease rotationEase;
    private string rotDir;

    [SerializeField] private float speed;

    [Header("Jump")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform boxPos;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask boxLayer;
    bool grounded;

    Vector3 startPosition;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void Start()
    {
        startPosition = transform.position;
        inputs.Subscribe(GameEnums.InputKey.Jump, InputActionPhase.Started, Jump);
    }

    private void OnDisable()
    {
        inputs.Unsubscribe(GameEnums.InputKey.Jump, InputActionPhase.Started, Jump);
    }

    private void Update()
    {
        movement = inputs.GetMoveValue();

        if (movement == Vector2.zero) { return; }

        if (Mathf.Sign(movement.x) == -1 && transform.eulerAngles.y != 180 && rotDir != "L")
        {
            rotDir = "L";
            Tween.Rotation(transform, targetRotation, rotationSpeed, rotationEase);
        }
        else if (Mathf.Sign(movement.x) == 1 && transform.eulerAngles.y != 0 && rotDir != "R")
        {
            rotDir = "R";
            Tween.Rotation(transform, Vector3.zero, rotationSpeed, rotationEase);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, movement.x * speed);
        grounded = Physics.OverlapBox(boxPos.position, boxSize, Quaternion.identity, boxLayer).Length > 0 ? true : false;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!grounded) { return; }

        rb.AddForce(new(0, jumpForce), ForceMode.Impulse);
    }

    public void Restart()
    {
        transform.position = startPosition;
        rb.linearVelocity = Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
