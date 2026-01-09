using System.Runtime.CompilerServices;
using System.Xml;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

[RequireComponent(typeof(MoveBehaviour))]
[RequireComponent(typeof(JumpBehaviour))]

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] protected MoveBehaviour _mb;
    [SerializeField] protected JumpBehaviour _jb;
    [SerializeField] protected Animator _animator;

    private InputSystem_Actions _inputActions;

    private float _groundCheckTimer = 0f;   // Temporitzador per la animació de salt

    public void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
        _jb = GetComponent<JumpBehaviour>();
        _animator = GetComponent<Animator>();
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Disable();
    }

    private void Update()
    {
        if (_animator != null)
        {
            if (_groundCheckTimer > 0)
            {
                _groundCheckTimer -= Time.deltaTime;
                _animator.SetBool("IsGrounded", false);
            }
            else
            {
                // Si el temporitzador ha acabat
                _animator.SetBool("IsGrounded", _jb.IsGrounded());
            }
        }
    }

    // Càmera
    public void OnLook(InputAction.CallbackContext context)
    {
        //
    }

    // Moviment
    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        _mb.SetInputDirection(moveInput);

        bool isMoving = moveInput.sqrMagnitude > 0.1f;
        if (_animator != null)
        {
            _animator.SetBool("IsWalking", isMoving);
        }
    }

    // Salt
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _jb.IsGrounded())
        {
            if (_animator != null && _jb.IsGrounded())
            {
                _animator.SetTrigger("Jump");
            }
            _groundCheckTimer = 0.5f;    // 0.3s de espera + 0.2s per despegar del terra

            _jb.DelayJump(0.3f);
        }
    }
}
