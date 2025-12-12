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
        // Obtenir dirreció de la càmera
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
            // --- ROTACIÓN SUAVE ---
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            Quaternion nextRotation = Quaternion.Slerp(_rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(nextRotation);

            // --- MOVIMIENTO ---
            Vector3 targetVelocity = moveDir * speed;

            // Aplicar al Rigidbody (manteniendo la gravedad en Y)
            // Nota: Si usas Unity 6, cambia 'velocity' por 'linearVelocity'
            _rb.linearVelocity = new Vector3(targetVelocity.x, _rb.linearVelocity.y, targetVelocity.z);
        }
        else
        {
            // Si no tocamos teclas, frenamos el movimiento horizontal pero dejamos caer la gravedad
            _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
        }

        /*
        Vector3 moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        Vector3 Velocity = moveDirection * speed;
        _rb.linearVelocity = new Vector3(Velocity.x, _rb.linearVelocity.y, Velocity.z);
        */
    }
}
