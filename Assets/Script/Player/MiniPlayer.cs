using Script.PowerUps.SecretKey;
using Script.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.Player
{
    public class MiniPlayer : MonoBehaviour
    {
        private PlayerControls _controls;

        private SpriteRenderer _player;
        [SerializeField] private SpriteRenderer miniplayerSp;
        [SerializeField] private Collider2D miniplayerCol;
        private Collider2D _playerCol;
        [SerializeField] private ColorEventChannel _colorEventChannel;
        [SerializeField] private GameCapabilityState _gameCapabilityState;
        [SerializeField] private GameColor revealColor = GameColor.ColorC;
        private bool _initialized;


        bool _isMiniPress = false;

        private void OnEnable()
        {
            if (_colorEventChannel) _colorEventChannel.OnColorChanged += OnStateChanged;
            if (_gameCapabilityState) _gameCapabilityState.OnKeyAcquired += OnSecretUnlocked;
            CheckVisibility();
        }

        private void OnDisable()
        {
            if (_colorEventChannel) _colorEventChannel.OnColorChanged -= OnStateChanged;
            if (_gameCapabilityState) _gameCapabilityState.OnKeyAcquired -= OnSecretUnlocked;
        }

        private void OnStateChanged(GameColor color) => CheckVisibility();
        private void OnSecretUnlocked() => CheckVisibility();

        private void CheckVisibility()
        {
            bool canShow = _gameCapabilityState.HasSecretKey &&
                          (_colorEventChannel.CurrentColor == revealColor);
            if (!_initialized)
            {
                _isMiniPress = canShow;
                LittlePlayer(canShow);
                _initialized = true;
                return;
            }
        }

        private void Awake()
        {
            _controls = new PlayerControls();
            _player = GetComponent<SpriteRenderer>();
            _playerCol = GetComponent<Collider2D>();
        }

        private void LittlePlayer(bool isPress)
        {
            _player.enabled = !isPress;
            _playerCol.enabled = !isPress;
            miniplayerSp.enabled = isPress;
            miniplayerCol.enabled = isPress;

            if (AudioManager.Instance != null)
            {
                if (isPress)
                    AudioManager.Instance.Play("Shrink");  //PILAS CAMBIAR NOMBRE
                else
                    AudioManager.Instance.Play("Grow");     //PILAS CAMBIAR NOMBRE
            }
        }

    }
}