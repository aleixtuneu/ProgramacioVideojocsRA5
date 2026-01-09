using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class MoveBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 15f;

    private Rigidbody _rb;
    private Vector2 _inputDirection;
    private Transform _cameraTransform;

    private bool _isAiming;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        if (Camera.main != null)
        {
            _cameraTransform = Camera.main.transform;
        }
    }

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

    private void Move()
    {
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

            Vector3 targetVelocity = moveDir * speed;
            _rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Si no hi ha input, el personatge es manté en la seva rotació
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
    }
}
