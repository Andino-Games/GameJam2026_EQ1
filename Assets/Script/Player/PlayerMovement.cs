using UnityEngine;

namespace Script.Player
{
    public class Player : MonoBehaviour
    {
        private PlayerControls _controls;
        private Vector2 _moveInput;
    

        [Header("Player Configuration")] 
        private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float acceleraton = 10f;
        [SerializeField] private float decelerator = 10f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform groundCheckPoint;
        [SerializeField] private float groundCheckRadius = 0.2f;
        private bool _isGrounded;

        private void Awake()
        {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();

            float targetSpeed = _moveInput.x * speed;
            float learp = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleraton : decelerator;

            float newVelocity = Mathf.Lerp(_rb.linearVelocity.x, targetSpeed, learp * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(newVelocity, _rb.linearVelocity.y);

            if (_moveInput.y > 0.5f)
            {
                Jump();
            }
        }

        private void Jump()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, groundLayer);
            if (_isGrounded)
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }

   
        private void OnDrawGizmos()
        {
            if (!groundCheckPoint) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, groundCheckRadius);
        }

        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => _controls.Player.Disable();
    }
}
