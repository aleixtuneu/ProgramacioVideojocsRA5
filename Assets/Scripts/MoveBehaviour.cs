using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class MoveBehaviour : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float turnSpeed = 0.5f;

    private Rigidbody _rb;
    private Vector2 _inputDirection;
    private Transform _cameraTransform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraTransform = Camera.main.transform;
    }

    public void SetInputDirection(Vector2 direction)
    {
        _inputDirection = direction;
    }

    private void FixedUpdate()
    {
        Move();
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

        if (moveDir.sqrMagnitude >= 0.01f)
        {
            // Rotació suau
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            Quaternion nextRotation = Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(nextRotation);

            // Moviment
            Vector3 targetVelocity = moveDir * speed;

            _rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Moviment si es pressionen tecles de moviment
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }
    }
}
