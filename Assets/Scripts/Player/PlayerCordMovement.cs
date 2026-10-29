using PrimeTween;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerCordMovement : MonoBehaviour
{
    [Inject] InputsManager inputs;
    [Inject] AudioManager audioManager;
    [Inject] GameManager game;
    private Rigidbody rb;

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

    [SerializeField] private float decreaseVolumeSpeed;
    [SerializeField] private Ease decreaseVolumeEase;


    [Header("Push")]
    [SerializeField] private float angleToPush;
    [SerializeField] private float pushForce;
    [SerializeField] private float pushTime;
    [SerializeField] private Ease pushEase;

    [Header("Rebobine")]
    [SerializeField] private Transform height;
    [SerializeField] private Transform position;
    [SerializeField] private float revoDelayTime;
    [SerializeField] private float revoHeigthTime;
    [SerializeField] private float revoPosTime;
    [SerializeField] private float revoRotTime;
    [SerializeField] private Ease revoEase;
    [SerializeField] GameObject check;
    private Tween revoTween;
    private bool rebobinating;
    private bool stop;

    private void Awake()
    {
        TryGetComponent(out rb);

        initialswingingForce = swingingForce;
    }

    private void OnEnable()
    {
        inputs.Subscribe(GameEnums.InputKey.Move, InputActionPhase.Canceled, ApplyForce);
        
    }

    private void OnDisable()
    {
        inputs.Unsubscribe(GameEnums.InputKey.Move, InputActionPhase.Canceled, ApplyForce);
        revoTween.timeScale = 0;
    }

    private void Update()
    {
        stop = game.time <= 0;
        if (stop ) 
        {
            rb.useGravity = false;
            return; 
        }
        movement = inputs.GetMoveValue();


        angle = Quaternion.Angle(transform.rotation, center.rotation);
        angle *= transform.eulerAngles.z < 180 ? 1 : -1;

        if (!rebobinating)
        {
            height.position = new(-1.312f * Mathf.Sign(angle), height.position.y, transform.position.z);
            position.position = new(position.position.x, position.position.y, transform.position.z);
        }

        if (Mathf.Abs(angle) < angleToRecenter && !recentering )
        {
            recentering = true;
            Tween.PositionX(transform, 0, recenterTime, recenterEase).OnComplete(() => recentering = false);
        }

        Screams();
        Push();

        if (!can_Rotate || !on_cord) { return; }
        transform.eulerAngles = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + (ForceValue * Time.deltaTime));

    }

    public void Push()
    {
        if (rebobinating) { return; }

        if (Mathf.Abs(angle) < angleToPush) { return; }
        can_Rotate = false;
        recentering = true;
        Tween.PositionX(transform, Mathf.Sign(angle) == -1 ? transform.position.x + pushForce : transform.position.x - pushForce, pushTime, pushEase);
    }

    private void FixedUpdate()
    {
        on_cord = Physics.OverlapBox(boxPos.position, boxSize, Quaternion.identity, boxLayer).Length > 0 ? true : false;
    }

    private void Screams()
    {
        if (rebobinating) { return; }

        if (Mathf.Abs(angle) < angleToRecenter) 
        {
            if (playing)
            {
                audioManager.StopSound(sfxSourceL, decreaseVolumeSpeed, decreaseVolumeEase);
                audioManager.StopSound(sfxSourceR, decreaseVolumeSpeed, decreaseVolumeEase);
                playing = false;
            }
            return; 
        }

        if (Mathf.Sign(angle) == 1 && !playing)
        {
            playing = true;
            audioManager.PlaySound(sfxSourceL, leftClip, true);
        }
        else if (Mathf.Sign(angle) == -1 && !playing)
        {
            playing = true;
            audioManager.PlaySound(sfxSourceR, rightClip, true);
        }
    }


    private void ApplyForce(InputAction.CallbackContext ctx)
    {
        if (!on_cord || rebobinating) { return; }

        can_Rotate = false;
        ForceValue = (swingingForce * -movement.x);
        Vector3 newRotation = new(transform.localEulerAngles.x, transform.localEulerAngles.y, transform.localEulerAngles.z + ForceValue);
        Tween.Rotation(transform, newRotation, swingingTime, swingingEase).OnComplete(() => can_Rotate = true);
        swingingForce += swingingForceIncreasement;

        rb.linearVelocity = new(rb.linearVelocity.x, rb.linearVelocity.y, Mathf.Abs(movement.x * moveSpeed));
    }

    public void Restart()
    {
        rebobinating = true;
        check.SetActive(false);
        Debug.Log("The Revo");
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        revoTween = Tween.Position(transform, height.position, revoHeigthTime, revoEase, 1, CycleMode.Restart, revoDelayTime).OnComplete(() =>
        {
            Tween.Position(transform, position.position, revoPosTime, revoEase).OnComplete(() =>
            {
                Tween.Rotation(transform, Vector3.zero, revoRotTime, revoEase).OnComplete(() =>
                {
                    swingingForce = initialswingingForce;
                    ForceValue = 0;
                    rb.useGravity = true;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    check.SetActive(true);
                    rebobinating = false;
                });
            });
        });
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.aliceBlue;
        Gizmos.DrawWireCube(boxPos.position, boxSize);
    }
}
