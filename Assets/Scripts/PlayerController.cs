using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MoveBehaviour))]

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [SerializeField] protected MoveBehaviour _mb;
    [SerializeField] protected Animator _animator;

    private InputSystem_Actions _inputActions;

    public void Awake()
    {
        _mb = GetComponent<MoveBehaviour>();
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
}
