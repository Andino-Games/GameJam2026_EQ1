using UnityEngine;
using UnityEngine.InputSystem;

public class ColorWheelController : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameObject _wheelVisuals;

    [Header("Configuración de Colores")]
    [SerializeField] private GameColor _colorTop;
    [SerializeField] private GameColor _colorRight;
    [SerializeField] private GameColor _colorLeft;

    private GameControls _controls;
    private bool _isSelecting;

    private void Awake()
    {
        _controls = new GameControls();
        if (_wheelVisuals == null) Debug.LogError("¡ERROR! No has asignado '_wheelVisuals' en el inspector.");
        else _wheelVisuals.SetActive(false);
    }

    private void OnEnable()
    {
        _controls.Gameplay.Enable();
        _controls.Gameplay.OpenColorWheel.started += OnClickStarted;
        _controls.Gameplay.OpenColorWheel.canceled += OnClickReleased;
        _controls.Gameplay.ResetColor.performed += OnDoubleClick;
        Debug.Log("--- SISTEMA DE INPUT ACTIVADO Y ESCUCHANDO ---");
    }

    private void OnDisable()
    {
        _controls.Gameplay.OpenColorWheel.started -= OnClickStarted;
        _controls.Gameplay.OpenColorWheel.canceled -= OnClickReleased;
        _controls.Gameplay.ResetColor.performed -= OnDoubleClick;
        _controls.Gameplay.Disable();
    }

    // ELIMINAMOS EL UPDATE QUE MOVÍA LA RUEDA
    // private void Update() { ... } 

    // --- LÓGICA DE DOBLE CLIC (RESET) ---
    private void OnDoubleClick(InputAction.CallbackContext context)
    {
        Debug.Log(">>> DOBLE CLIC DETECTADO: Reseteando a Color Base (None) <<<");
        _isSelecting = false;
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);
        _colorChannel.RaiseColorChanged(GameColor.None);
    }

    // --- LÓGICA DE CLIC NORMAL (SELECCIÓN) ---
    private void OnClickStarted(InputAction.CallbackContext context)
    {
        _isSelecting = true;
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        
        if (_wheelVisuals != null)
        {
            // Solo movemos la rueda AL INICIO del clic. Luego se queda quieta.
            _wheelVisuals.transform.position = mousePos;
            _wheelVisuals.SetActive(true);
        }
        Debug.Log(">>> CLIC INICIADO: Rueda fijada en posición " + mousePos + " <<<");
    }

    private void OnClickReleased(InputAction.CallbackContext context)
    {
        if (!_isSelecting) return;

        _isSelecting = false;
        if (_wheelVisuals != null) _wheelVisuals.SetActive(false);

        GameColor selected = CalculateColorFromMouse();
        
        if (selected != GameColor.None)
        {
            Debug.Log($"<color=green>COLOR CONFIRMADO: {selected}</color>");
            _colorChannel.RaiseColorChanged(selected);
        }
        else
        {
            Debug.Log("<color=yellow>Zona muerta (No moviste el mouse lo suficiente desde el centro)</color>");
        }
    }

    private GameColor CalculateColorFromMouse()
    {
        Vector2 mousePos = _controls.Gameplay.MousePosition.ReadValue<Vector2>();
        Vector2 center = _wheelVisuals.transform.position; // El centro ahora es fijo
        Vector2 direction = mousePos - center;

        // Debug visual de la distancia
        Debug.Log($"Distancia del centro: {direction.magnitude} píxeles");

        if (direction.magnitude < 20f) return GameColor.None; // Aumenté un poco el margen a 20px

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        Debug.Log($"Ángulo: {angle:F2}°");

        if (angle >= 45 && angle < 135) return _colorTop;
        if (angle >= 135 && angle < 225) return _colorLeft;
        else return _colorRight;
    }
}