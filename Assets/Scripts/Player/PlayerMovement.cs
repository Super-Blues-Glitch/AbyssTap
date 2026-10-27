using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [Inject] InputsManager inputs;
    [Inject] AudioManager audioManager;
    private Rigidbody rb;


    [Header("Swinging")]
    [SerializeField] private float swingingForce;
    [SerializeField] private float swingingTime;
    [SerializeField] private Ease swingingEase;
    [SerializeField] private float swingingForceIncreasement;
    [SerializeField] private float angleToActivateXMoveRight;
    [SerializeField] private float angleToActivateXMoveLeft;
    private float ForceValue;

    [Header("Movement")]
    [SerializeField] private Vector2 movement;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float recenterTime;
    [SerializeField] private Ease recenterEase;

    [Header("Recenter")]
    private bool can_Rotate;
    private bool recentering;

    [Header("Audio")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip leftClip;
    [SerializeField] private AudioClip rightClip;

    public Transform leftLimit, rightLimit;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void Start()
    {
        inputs.Subscribe(GameEnums.InputKey.Move, InputActionPhase.Canceled, ApplyForce);
    }

    private void Update()
    {
        movement = inputs.GetMoveValue();

        if (transform.eulerAngles.z <= leftLimit.eulerAngles.z || transform.eulerAngles.z >= rightLimit.eulerAngles.z)
        {
            Debug.LogError("Recentering");
            recentering = true;
            Tween.PositionX(transform, 0, recenterTime, recenterEase).OnComplete(() => recentering = false);
        }
        else
        {
            audioManager.PlaySound(sfxSource, transform.eulerAngles.z > leftLimit.eulerAngles.z ? leftClip : transform.eulerAngles.z < rightLimit.eulerAngles.z ? rightClip : null);
        }

        if (!can_Rotate) { return; }
        transform.eulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + (ForceValue * Time.deltaTime));

    }

    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        can_Rotate = false;
        ForceValue = (swingingForce * -movement.x);
        Vector3 newRotation = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + ForceValue);
        Tween.Rotation(transform, newRotation, swingingTime, swingingEase).OnComplete(() => can_Rotate = true);
        swingingForce += swingingForceIncreasement;

        rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, Mathf.Abs(movement.x * moveSpeed));
    }
}
