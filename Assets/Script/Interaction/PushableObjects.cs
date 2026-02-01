using Script.Interaction.Abstractions;
using UnityEngine;

namespace Script.Interaction
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PushableObjects : MonoBehaviour, IStayInteractable
    {
        [SerializeField] private float pushForce;
        
        private Rigidbody2D _rb;
        private IInteractor _interactor;
        private Transform _interactorTransform;
        private bool _isPushing;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        
        public void OnInteractStart(IInteractor interactor)
        {
            _interactor = interactor;
            _interactorTransform = (interactor as MonoBehaviour)?.transform;
            _isPushing = true;

            // Audio Events
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("Grab");   //PILAS CAMBIAR NOMBRE
                AudioManager.Instance.Play("DragLoop");   //PILAS CAMBIAR NOMBRE
            }
        }
        
        public void OnInteractEnd(IInteractor interactor)
        {
            if (_interactor == interactor)
            {
                _interactor = null;
                _isPushing = false;

                // Audio Events
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Stop("DragLoop");    //PILAS CAMBIAR NOMBRE
                    AudioManager.Instance.Play("Release");     //PILAS CAMBIAR NOMBRE
                }
            }
        }
        
        private void FixedUpdate()
        {
            if (!_isPushing) return;
            if (!_interactorTransform) return;

            Vector3 direction = _interactorTransform.position - transform.position;
            _rb.AddForce(direction.normalized * pushForce);
        }
    }
}