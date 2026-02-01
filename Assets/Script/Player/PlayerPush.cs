using Script.Interaction.Abstractions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class PlayerPush : MonoBehaviour, IInteractor
    {
        [SerializeField] private float grabDistance = 1f;
        private PlayerControls _playerInput;
        [SerializeField] private LayerMask grabLayerMask;
        
        private bool _isPushing;
        private IStayInteractable _pushable;
        
        private void Awake()
        {
            _playerInput = new PlayerControls();
        }

        private void OnEnable()
        {
            _playerInput.Player.Enable();
            _playerInput.Player.Grab.performed += OnGrabPerformed;
        }

        private void OnDisable()
        {
            _playerInput.Player.Grab.performed -= OnGrabPerformed;
            _playerInput.Player.Disable();
        }

        private void OnGrabPerformed(InputAction.CallbackContext obj)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale.x, grabDistance, grabLayerMask);
            if (!hit.collider) return;
            var isPushable = hit.collider.TryGetComponent(out _pushable);
            if (!isPushable) return;
            
            if (!_isPushing)
            {
                _pushable?.OnInteractStart(this);
                _isPushing = true;
            }
            else if (_isPushing)
            {
                _pushable?.OnInteractEnd(this);
                _isPushing = false;
            }
            
        }

        #region Debug

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * grabDistance);
        }

        #endregion
    }
}