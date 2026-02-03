using Script.Interaction.Abstractions;
using UnityEngine;
using UnityEngine.InputSystem;
using Script.UI;
using Script.PowerUps.SecretKey;


namespace Script.Player
{
    public class PlayerPush : MonoBehaviour, IInteractor
    {
        [SerializeField] private float grabDistance = 1f;
        [SerializeField] private LayerMask grabLayerMask;
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameCapabilityState capabilityState;
        [SerializeField] private GameColor revealColor = GameColor.ColorC;
        [SerializeField] private Animator anim;
        private IStayInteractable _pushable;
        private Transform _pushedObjectTransform; // Para guardar la referencia del transform
        private bool _isCurrentlyPushing = false;
        public bool IsCurrentlyPushing
        {
            get { return _isCurrentlyPushing; }
        }

        private void OnEnable()
        {
            if (colorChannel) colorChannel.OnColorChanged += OnStateChanged;
            if (capabilityState) capabilityState.OnKeyAcquired += OnSecretUnlocked;
        }

        private void OnDisable()
        {
            if (colorChannel) colorChannel.OnColorChanged -= OnStateChanged;
            if (capabilityState) capabilityState.OnKeyAcquired -= OnSecretUnlocked;
        }

        private void Update()
        {
            CheckPushState();

            if (_isCurrentlyPushing && _pushedObjectTransform != null)
            {
                UpdateBlockPosition();
            }
        }

        private void OnStateChanged(GameColor color) => CheckPushState();
        private void OnSecretUnlocked() => CheckPushState();

        private void CheckPushState()
        {
            // Condiciones para poder empujar
            bool conditionsMet = capabilityState.HasSecretKey && (colorChannel.CurrentColor == revealColor);

            if (conditionsMet && !_isCurrentlyPushing)
            {
                TryStartPush();
               
            }
            else if (!conditionsMet && _isCurrentlyPushing)
            {
                StopPush();
            }
        }

        private void TryStartPush()
        {
            // Raycast hacia donde mira el jugador
            Vector2 direction = Vector2.right * transform.localScale.x;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, grabDistance, grabLayerMask);

            if (hit.collider != null && hit.collider.TryGetComponent(out _pushable))
            {
                _pushedObjectTransform = hit.collider.transform;
                _isCurrentlyPushing = true;
                anim.SetBool("Grab", true);
                _pushable.OnInteractStart(this);
                
            }
        }

        private void StopPush()
        {
            if (_pushable != null)
            {
                anim.SetBool("Grab", false);

                _pushable.OnInteractEnd(this);
            }
            _pushable = null;
            _pushedObjectTransform = null;
            _isCurrentlyPushing = false;
           
        }

        private void UpdateBlockPosition()
        {
            // Calculamos la posición objetivo (enfrente del jugador)
            Vector3 targetPos = transform.position + (Vector3.right * transform.localScale.x * grabDistance);

            // Mantenemos la Y original del bloque para que no "vuele" si no quieres
            targetPos.y = _pushedObjectTransform.position.y;

            // Movimiento suave o directo
            _pushedObjectTransform.position = targetPos;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * transform.localScale.x * grabDistance));
        }
    }
}