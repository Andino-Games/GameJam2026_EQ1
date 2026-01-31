using UnityEngine;
using UnityEngine.InputSystem;


public class Player : MonoBehaviour
{
   private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        controls = new PlayerControls();
        
        // Suscribirse a eventos (como el salto)
        // controls.Player.Jump.performed += ctx => Jump();
    }

    private void Update()
    {
        // Leer valores continuos (como el movimiento)
        moveInput = controls.Player.Move.ReadValue<Vector2>();
        transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * Time.deltaTime * 5f);
    }

    // private void Jump()
    // {
    //     Debug.Log("¡Saltaste!");
    // }

    // Es vital activar y desactivar los controles
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
}
