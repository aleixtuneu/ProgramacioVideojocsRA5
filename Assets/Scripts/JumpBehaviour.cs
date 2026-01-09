using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class JumpBehaviour : MonoBehaviour
{
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundLayer;

    // Configuració del Spherecast
    [SerializeField] private float sphereCastRadius = 0.4f;
    [SerializeField] private float sphereCastDistance = 0.1f;
    [SerializeField] private float bodyHeightOffset = 0.5f;

    private Rigidbody _rb;
    private bool _isGrounded;
    private bool _isJumping = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        CheckGround();
    }

    // Comprovar si toca el terra
    private void CheckGround()
    {
        // Origen del Spherecast
        Vector3 origin = transform.position + Vector3.up * bodyHeightOffset;

        // Distancia del Spherecast
        float dist = bodyHeightOffset + sphereCastDistance;

        _isGrounded = Physics.SphereCast(origin, sphereCastRadius, Vector3.down, out RaycastHit hit, dist, groundLayer);
    }

    // Saltar
    public void Jump()
    {
        Vector3 currentVel = _rb.linearVelocity;
        _rb.linearVelocity = new Vector3(currentVel.x, 0f, currentVel.z);
        _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Delay de salt
    public void DelayJump(float delay)
    {
        if (_isGrounded && !IsInvoking(nameof(Jump)))
        {
            Invoke(nameof(Jump), delay);
        }
    }

    public bool IsGrounded()
    {
        return _isGrounded; 
    }

    // Visualitzar radi i distància
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 origin = transform.position + Vector3.up * bodyHeightOffset;
        Gizmos.DrawWireSphere(origin + Vector3.down * (bodyHeightOffset + sphereCastDistance), sphereCastRadius);
    }
}
