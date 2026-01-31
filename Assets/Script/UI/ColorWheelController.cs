using UnityEngine;
using UnityEngine.InputSystem;

public class ColorWheelController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameCapabilityState _capabilityState; // Referencia al estado secreto
    [SerializeField] private GameObject _wheelVisuals;

    [Header("Configuración de Colores")]
    [SerializeField] private GameColor _colorTop;
    [SerializeField] private GameColor _colorRight;
    [SerializeField] private GameColor _colorLeft;

    private GameControls _controls;
    private bool _isSelecting;

    private void Awake()
    {
        // 1. Limpieza de memoria (Importante para evitar bugs al reiniciar)
        if (_colorChannel != null) _colorChannel.ResetState();
        if (_capabilityState != null) _capabilityState.ResetState();

        _controls = new GameControls();
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);
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
        
        if (_wheelVisuals != null)
        {
            _wheelVisuals.transform.position = screenCenter;
            _wheelVisuals.SetActive(true);
        }

        // Opcional: Llevar el mouse al centro
        if (Mouse.current != null) Mouse.current.WarpCursorPosition(screenCenter);
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        if (!_isSelecting) return;

        _isSelecting = false;
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);

        GameColor selected = CalculateColorFromMouse();
        
        if (selected != GameColor.None)
        {
            _colorChannel.RaiseColorChanged(selected);
        }
    }

    private void OnDoubleClick(InputAction.CallbackContext context)
    {
        _isSelecting = false;
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);
        _colorChannel.RaiseColorChanged(GameColor.None); // Volver a gris
    }

    private GameColor CalculateColorFromMouse()
    {
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        Vector2 center = _wheelVisuals.transform.position;
        Vector2 direction = mousePos - center;

        if (direction.magnitude < 20f) return GameColor.None;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        if (angle >= 45 && angle < 135) return _colorTop;
        if (angle >= 135 && angle < 225) return _colorLeft;
        else return _colorRight;
    }
}