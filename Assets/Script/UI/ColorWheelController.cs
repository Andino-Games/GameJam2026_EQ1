using Script.PowerUps;
using Script.PowerUps.SecretKey;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.UI
{
    public class ColorWheelController : MonoBehaviour
    {
        [Header("Sistemas")]
        [SerializeField] private ColorEventChannel colorChannel;
        [SerializeField] private GameCapabilityState secretState; 
        [SerializeField] private ColorCapabilityState colorState; // Estado de desbloqueo

        [Header("UI Visuals")]
        [SerializeField] private GameObject wheelVisuals; // La rueda completa (un solo objeto)

        [Header("Configuración de Colores (Lógica)")]
        [SerializeField] private GameColor colorTop = GameColor.ColorA;
        [SerializeField] private GameColor colorLeft = GameColor.ColorB;
        [SerializeField] private GameColor colorRight = GameColor.ColorC;

        private GameControls _controls;
        private bool _isSelecting;
        private bool _enabled;

        public void Disable()
        {
            _enabled = false;
        }
        
        public void Enable()
        {
            _enabled = true;
        }

        private void Awake()
        {
            // Limpieza de memoria al iniciar
            if (colorChannel) colorChannel.ResetState();
            if (secretState) secretState.ResetState();
            if (colorState) colorState.ResetState();

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
            if (!_enabled) return;
            
            _isSelecting = true;
        
            // Centrar en pantalla
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        
            if (wheelVisuals)
            {
                wheelVisuals.transform.position = screenCenter;
                wheelVisuals.SetActive(true);
            }

            if (Mouse.current != null) Mouse.current.WarpCursorPosition(screenCenter);

            if (AudioManager.Instance != null) AudioManager.Instance.Play("WheelOpen");
        }

        private void OnClickReleased(InputAction.CallbackContext context)
        {
            if (!_enabled) return;
            
            if (!_isSelecting) return;

            _isSelecting = false;
            if (wheelVisuals) wheelVisuals.SetActive(false);

            GameColor selected = CalculateColorFromMouse();
        
            // LÓGICA DE BLOQUEO:
            // Solo permitimos cambiar si el color es None (gris) O si tenemos el color desbloqueado
            if (selected != GameColor.None && colorState.IsUnlocked(selected))
            {
                if (AudioManager.Instance != null) AudioManager.Instance.Play("WheelSelect");
                colorChannel.RaiseColorChanged(selected);
            }
            else
            {
                // Si intentas seleccionar algo bloqueado o nada
                if (AudioManager.Instance != null) AudioManager.Instance.Play("WheelClose");
            }
        }

        private void OnDoubleClick(InputAction.CallbackContext context)
        {
            if (!_enabled) return;
            
            _isSelecting = false;
            if (wheelVisuals) wheelVisuals.SetActive(false);
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