using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerCordMovement : MonoBehaviour
{
    [Inject] InputsManager inputs;
    [Inject] AudioManager audioManager;
    private Rigidbody rb;
    private Rebobinator revo;

    [SerializeField] private float angle;

    [Header("Swinging")]
    [SerializeField] private float swingingForce;
    private float initialswingingForce;
    [SerializeField] private float swingingTime;
    [SerializeField] private Ease swingingEase;
    [SerializeField] private float swingingForceIncreasement;
    private float ForceValue;

    [Header("Movement")]
    [SerializeField] private Vector2 movement;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float recenterTime;
    [SerializeField] private Ease recenterEase;

    [Header("Recenter")]
    [SerializeField] private Transform center;
    [SerializeField] private float angleToRecenter;
    private bool can_Rotate;
    private bool recentering;

    [Header("Detection")]
    [SerializeField] private Transform boxPos;
    [SerializeField] private Vector3 boxSize;
    [SerializeField] private LayerMask boxLayer;
    private bool on_cord;

    [Header("Audio")]
    private bool playing;
    [SerializeField] private AudioSource sfxSourceL;
    [SerializeField] private AudioSource sfxSourceR;
    [SerializeField] private AudioClip leftClip;
    [SerializeField] private AudioClip rightClip;

    [Header("Limits")]

    [Header("Push")]
    [SerializeField] private Transform pusherLeft, pusherRight;
    [SerializeField] private float pushForce;
    [SerializeField] private float pushTime;
    [SerializeField] private Ease pushEase;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out revo);

        initialswingingForce = swingingForce;
    }

    private void OnEnable()
    {
        inputs.Subscribe(GameEnums.InputKey.Move, InputActionPhase.Canceled, ApplyForce);
        revo.RevoDone += Restart;
    }

    private void OnDisable()
    {
        inputs.Unsubscribe(GameEnums.InputKey.Move, InputActionPhase.Canceled, ApplyForce);
        revo.RevoDone -= Restart;
    }

    private void Update()
    {
        movement = inputs.GetMoveValue();

        angle = Quaternion.Angle(transform.rotation, center.rotation);
        angle *= transform.eulerAngles.z < 180 ? 1 : -1;

        pusherLeft.position = new(pusherLeft.position.x, pusherLeft.position.y, transform.position.z);
        pusherRight.position = new(pusherRight.position.x, pusherRight.position.y, transform.position.z);

        if (Mathf.Abs(angle) < angleToRecenter && !recentering)
        {
            recentering = true;
            Tween.PositionX(transform, 0, recenterTime, recenterEase).OnComplete(() => recentering = false);
        }

        Screams();

        if (!can_Rotate || !on_cord) { return; }
        transform.eulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + (ForceValue * Time.deltaTime));

    }

    public void Push()
    {
        can_Rotate = false;
        recentering = true;
        Tween.PositionX(transform, Mathf.Sign(angle) == -1 ? transform.position.x + pushForce : transform.position.x - pushForce, pushTime, pushEase);
        Screams();
    }

    private void FixedUpdate()
    {
        on_cord = Physics.OverlapBox(boxPos.position, boxSize, Quaternion.identity, boxLayer).Length > 0 ? true : false;
    }

    private void Screams()
    {
        if (Mathf.Abs(angle) < angleToRecenter) 
        { 
            audioManager.StopSound(sfxSourceL);
            audioManager.StopSound(sfxSourceR);
            playing = false;
            return; 
        }
        Debug.LogWarning("Sonando");

        if (Mathf.Sign(angle) == 1)
        {
            playing = true;
            audioManager.PlaySound(sfxSourceL, leftClip, true);
        }
        else
        {
            playing = true;
            audioManager.PlaySound(sfxSourceR, rightClip, true);
        }
    }


    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        if (!on_cord) { return; }

        can_Rotate = false;
        ForceValue = (swingingForce * -movement.x);
        Vector3 newRotation = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + ForceValue);
        Tween.Rotation(transform, newRotation, swingingTime, swingingEase).OnComplete(() => can_Rotate = true);
        swingingForce += swingingForceIncreasement;

        rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, Mathf.Abs(movement.x * moveSpeed));
    }

    private void Restart()
    {
        swingingForce = initialswingingForce;
        ForceValue = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
