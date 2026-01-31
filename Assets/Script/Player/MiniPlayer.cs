using UnityEngine;

namespace Script.Player
{
    public class MiniPlayer : MonoBehaviour
    {
        private PlayerControls _controls;

        private SpriteRenderer _player;
        [SerializeField] private SpriteRenderer miniplayerSp;
        [SerializeField] private Collider2D miniplayerCol;
        private Collider2D _playerCol;

        bool _isMiniPress;

        private void Awake()
        {
            _controls = new PlayerControls();
            _player = GetComponent<SpriteRenderer>();
            _playerCol = GetComponent<Collider2D>();
        }

        private void Update()
        {
            if (_controls.Player.PowerUp.triggered)
            {
                _isMiniPress = !_isMiniPress;
                LittlePlayer(_isMiniPress);
            }
        }

        private void LittlePlayer(bool isPress)
        {
            _player.enabled = !isPress;
            _playerCol.enabled = !isPress;
            miniplayerSp.enabled = isPress;
            miniplayerCol.enabled = isPress;
        }

        private void OnEnable() => _controls.Player.Enable();

        private void OnDisable() => _controls.Player.Disable();
    }
}
