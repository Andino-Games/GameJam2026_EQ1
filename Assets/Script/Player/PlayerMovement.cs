using UnityEngine;

namespace Script.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private PlayerControls _controls;
        private Vector2 _moveInput;
        private MiniPlayer _miniPlayer;

        [Header("Player Configuration")]
        private Rigidbody2D _rb;
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float acceleraton = 10f;
        [SerializeField] private float decelerator = 10f;
        private PlayerPush _pushScript;
        SpriteRenderer sp;
        Animator anim;
        private bool _isGrounded;

        private void Awake()
        {
            _controls = new PlayerControls();
            _rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            sp = GetComponent<SpriteRenderer>();
            _miniPlayer = GetComponent<MiniPlayer>();
            _pushScript = GetComponent<PlayerPush>();
        }

        private void FixedUpdate()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();

            float targetSpeed = _moveInput.x * speed;
            float learp = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleraton : decelerator;

            HandleVisuals();

            float newVelocity = Mathf.Lerp(_rb.linearVelocity.x, targetSpeed, learp * Time.fixedDeltaTime);
            _rb.linearVelocity = new Vector2(newVelocity, _rb.linearVelocity.y);

            if (_moveInput.y > 0.5f) Jump();
        }

        private void HandleVisuals()
        {
            bool isMoving = Mathf.Abs(_moveInput.x) > 0.01f;

            anim.SetBool("Run", isMoving);

            if (_miniPlayer.miniplayerAnim != null)
                _miniPlayer.miniplayerAnim.SetBool("Run", isMoving);

            if (isMoving)
            {
                if (_pushScript != null && !_pushScript.IsCurrentlyPushing)
                {
                    bool lookLeft = _moveInput.x < 0;

                    sp.flipX = lookLeft;

                    if (_miniPlayer.miniplayerSp != null)
                        _miniPlayer.miniplayerSp.flipX = lookLeft;
                }
                // Si IsCurrentlyPushing es true, el código del flip se ignora 
                // y el personaje mantiene la dirección que tenía al empezar el agarre.
            }
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                // Sincronizar Animación de Salto
                anim.SetBool("Jump", true);
                if (_miniPlayer.miniplayerAnim != null)
                    _miniPlayer.miniplayerAnim.SetBool("Jump", true);

                _isGrounded = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                anim.SetBool("Jump", false);
                if (_miniPlayer.miniplayerAnim != null)
                    _miniPlayer.miniplayerAnim.SetBool("Jump", false);

                _isGrounded = true;
            }
        }

        private void OnEnable() => _controls.Player.Enable();
        private void OnDisable() => _controls.Player.Disable();
    }
}
