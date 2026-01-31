using UnityEngine;

public class SecretPlatform : MonoBehaviour
{
    [SerializeField] private ColorEventChannel _colorChannel;
    [SerializeField] private GameCapabilityState _capabilityState;
    [SerializeField] private GameColor _revealColor = GameColor.ColorC;

    private Collider2D _collider;
    private Renderer _renderer;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged += OnStateChanged;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired += OnSecretUnlocked;
        CheckVisibility();
    }

    private void OnDisable()
    {
        if (_colorChannel != null) _colorChannel.OnColorChanged -= OnStateChanged;
        if (_capabilityState != null) _capabilityState.OnKeyAcquired -= OnSecretUnlocked;
    }

    private void OnStateChanged(GameColor color) => CheckVisibility();
    private void OnSecretUnlocked() => CheckVisibility();

    private void CheckVisibility()
    {
        // Solo aparece si: Tienes la llave Y es el color correcto
        bool canShow = _capabilityState.HasSecretKey && 
                       (_colorChannel.CurrentColor == _revealColor);
        
        if (_collider != null) _collider.enabled = canShow;
        if (_renderer != null) _renderer.enabled = canShow;
    }
}