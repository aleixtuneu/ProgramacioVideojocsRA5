using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class MoveBehaviour : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 3.5f;
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float turnSpeed = 10f;

    private Rigidbody _rb;
    private Vector2 _inputDirection;
    private Transform _cameraTransform;
    private bool _isSprinting;
    private bool _isAiming;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

    // Obtenir direcció de moviment
    public Vector2 GetInputDirection() => _inputDirection;

    public void SetInputDirection(Vector2 direction)
    {
        _inputDirection = direction;
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void SetAiming(bool aiming)
    {
        _isAiming = aiming;
    }

    public void SetSprinting(bool sprinting)
    {
        _isSprinting = sprinting;
    }

    private void Move()
    {
        // Resetejar velocitat de rotació
        _rb.angularVelocity = Vector3.zero;

        // Obtenir dirrecions de la càmera
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;

        // Ignorar si la càmera mira amunt o avall
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        // Calcular direcció de moviment
        Vector3 moveDir = (camForward * _inputDirection.y + camRight * _inputDirection.x).normalized;

        // Determinar si camina o corre, si apunta només pot caminar
        float currentSpeed = (_isSprinting && !_isAiming) ? runSpeed : walkSpeed;

        // Si apunta, el personatge sempre mira on la càmera
        if (_isAiming)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForward);
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }

        if (moveDir.sqrMagnitude >= 0.01f)
        {
            // Si no apunta rotació normal
            if (!_isAiming)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
            }

            Vector3 targetVelocity = moveDir * currentSpeed;
            //_rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);
            _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z), 0.15f);
        }
        else
        {
            // Si no hi ha input, el personatge es manté en la seva rotació
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
    }
}
