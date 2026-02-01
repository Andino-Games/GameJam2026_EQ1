using Script.PowerUps.SecretKey;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.UI
{
    public class ColorWheelController : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameCapabilityState capabilityState; // Referencia al estado secreto
        [SerializeField] private GameObject wheelVisuals;

        [Header("Configuración de Colores")]
        [SerializeField] private GameColor colorTop;
        [SerializeField] private GameColor colorRight;
        [SerializeField] private GameColor colorLeft;

        private GameControls _controls;
        private bool _isSelecting;

        private void Awake()
        {
            // 1. Limpieza de memoria (Importante para evitar bugs al reiniciar)
            if (colorChannel) colorChannel.ResetState();
            if (capabilityState) capabilityState.ResetState();

            _controls = new GameControls();
            if (wheelVisuals) wheelVisuals.SetActive(false);
        }

        private void OnEnable()
        {
            _controls.Gameplay.Enable();
            _controls.Gameplay.OpenColorWheel.started += OnClickStarted;
            _controls.Gameplay.OpenColorWheel.canceled += OnClickReleased;
            _controls.Gameplay.ResetColor.performed += OnDoubleClick;
        }

        private void OnDisable()
        {
            _controls.Gameplay.OpenColorWheel.started -= OnClickStarted;
            _controls.Gameplay.OpenColorWheel.canceled -= OnClickReleased;
            _controls.Gameplay.ResetColor.performed -= OnDoubleClick;
            _controls.Gameplay.Disable();
        }

        private void OnClickStarted(InputAction.CallbackContext context)
        {
            _isSelecting = true;
        
            // Forzar aparición en el CENTRO de la pantalla
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
            if (wheelVisuals)
            {
                wheelVisuals.transform.position = screenCenter;
                wheelVisuals.SetActive(true);
            }

            // Opcional: Llevar el mouse al centro
            if (Mouse.current != null) Mouse.current.WarpCursorPosition(screenCenter);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("WheelOpen");   //PILAS CAMBIAR NOMBRE
            }
        }

        private void OnClickReleased(InputAction.CallbackContext context)
        {
            if (!_isSelecting) return;

            _isSelecting = false;
            if (wheelVisuals) wheelVisuals.SetActive(false);

            GameColor selected = CalculateColorFromMouse();
        
            if (selected != GameColor.None)
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Play("WheelSelect");    //PILAS CAMBIAR NOMBRE
                }
                colorChannel.RaiseColorChanged(selected);
            }
            else
            {
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.Play("WheelClose");    //PILAS CAMBIAR NOMBRE
                }
            }
        }

        private void OnDoubleClick(InputAction.CallbackContext context)
        {
            _isSelecting = false;
            if (wheelVisuals) wheelVisuals.SetActive(false);
            
            // Nota: No agregamos audio aquí porque AudioManager ya reproduce "ColorReset"
            // automáticamente al detectar GameColor.None
            colorChannel.RaiseColorChanged(GameColor.None);
        }

        private GameColor CalculateColorFromMouse()
        {
            Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
            Vector2 center = wheelVisuals.transform.position;
            Vector2 direction = mousePos - center;

            if (direction.magnitude < 20f) return GameColor.None;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            return angle switch
            {
                >= 45 and < 135 => colorTop,
                >= 135 and < 225 => colorLeft,
                _ => colorRight
            };
        }
    }
}