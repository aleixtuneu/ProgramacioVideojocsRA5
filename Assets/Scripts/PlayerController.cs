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

    [SerializeField] private GameObject _thirdPersonCamera;
    [SerializeField] protected GameObject _firstPersonCamera;
    private bool _isAiming;
    public static bool keyCollected = false;

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

            // Si deixa de moure's, no pot córrer
            if (moveInput.sqrMagnitude <= 0.1f)
            {
                _animator.SetBool("IsRunning", false);
            }
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

    // Apuntar
    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //
            SyncCameras(); // Sincronitzar càmeres abans de fer el canvi
            //
            _isAiming = true;
            _firstPersonCamera.GetComponent<CinemachineCamera>().Priority = 20;
        }
        else if (context.canceled)
        {
            _isAiming = false;
            _firstPersonCamera.GetComponent<CinemachineCamera>().Priority = 5;
        }

        _mb.SetAiming(_isAiming);
    }

    // Sincronitzar càmeres
    public void SyncCameras()
    {
        // Obtenir components de PanTilt
        var panTilt3P = _thirdPersonCamera.GetComponent<CinemachinePanTilt>();
        var panTilt1P = _firstPersonCamera.GetComponent<CinemachinePanTilt>();

        if (panTilt3P != null && panTilt1P != null)
        {
            // Copiar valors perquè comencin des del mateix punt
            panTilt1P.PanAxis.Value = panTilt3P.PanAxis.Value;
            panTilt1P.TiltAxis.Value = panTilt3P.TiltAxis.Value;
        }
    }

    // Córrer
    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            _mb.SetSprinting(true);
            UpdateSprintAnimation(true);
        }
        else if (context.canceled)
        {
            _mb.SetSprinting(false);
            UpdateSprintAnimation(false);
        }
    }

    // Actualitzar animació de córrer
    private void UpdateSprintAnimation(bool sprinting)
    {
        if (_animator != null)
        {
            // Nomes corre si hi ha moviment i no apunta
            bool canRun = sprinting && _mb.GetInputDirection().sqrMagnitude > 0.1f;
            _animator.SetBool("IsRunning", canRun);
        }
    }

    // Obtenir si el jugador té la clau recollida
    public bool HasKey()
    {
        return GameManager.Instance.IsKeyCollected;
    }
}
