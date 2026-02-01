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
        SpriteRenderer sp;
        Animator anim;
        private bool _isGrounded;

        private void Awake()
        {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sp = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();

            float targetSpeed = _moveInput.x * speed;
            float learp = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleraton : decelerator;
            if(_moveInput.x !=0) 
            {
                anim.SetBool("Run", true);
                if(_moveInput.x > 0)
                {
                    sp.flipX = false;
                }
                else
                {
                    sp.flipX = true;

                 }
            }
            else
            {
                anim.SetBool("Run", false);

            }

            float newVelocity = Mathf.Lerp(_rb.linearVelocity.x, targetSpeed, learp * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(newVelocity, _rb.linearVelocity.y);

            if (_moveInput.y > 0.5f)
            {
                Jump();
            }
        }
        
        private void Jump()
        {            
            if (_isGrounded)
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                anim.SetBool("Jump", true);
                _isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
            {

                anim.SetBool("Jump", false);
                _isGrounded = true;
            }
        }
        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => _controls.Player.Disable();
    }
}
