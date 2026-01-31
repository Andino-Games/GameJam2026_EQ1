using UnityEngine;
using UnityEngine.InputSystem;


public class MiniPlayer : MonoBehaviour
{
    private PlayerControls controls;

    SpriteRenderer player;
    [SerializeField] SpriteRenderer _MiniplayerSp;
    [SerializeField] Collider2D _MiniplayerCol;
    Collider2D playerCol;

    bool _isMiniPress = false;
    private void Awake()
    {
        controls = new PlayerControls();
        player = GetComponent<SpriteRenderer>();
        playerCol = GetComponent<Collider2D>();
    }
  

    private void Update()
    {
       if (controls.Player.PowerUp.triggered)
        {
            _isMiniPress = !_isMiniPress;
            LittlePlayer(_isMiniPress);
        }

    }
    private void LittlePlayer(bool _isPress)
    {
        player.enabled = !_isPress;
        playerCol.enabled = !_isPress;
        _MiniplayerSp.enabled = _isPress;
        _MiniplayerCol.enabled = _isPress;
        //Debug.Log("Presiono");
    }
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
}
