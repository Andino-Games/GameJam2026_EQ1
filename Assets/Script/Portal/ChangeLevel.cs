using Script.UI;
using System.Collections;
using Script.Player;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private ScreenFader screenFader;
    [SerializeField] private GameObject finalImage;
    public bool final;
    private PlayerMovement player;
    

    private void Start()
    {
        player = FindFirstObjectByType<PlayerMovement>();
        if (screenFader)
        {
            screenFader.gameObject.SetActive(false);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fader());
                    
        }
    }
    private IEnumerator Fader()
    {
        // 1. Activar el Fader
        if (screenFader) screenFader.gameObject.SetActive(true);

        if (player) player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // 4. Fade Out (Pantalla a Negro)
        if (screenFader) yield return screenFader.FadeOut();
        else yield return new WaitForSeconds(0.5f);

        if (!final)
        {
            player.transform.position = playerPosition.position;

        }
        else
        {
            finalImage.SetActive(true);
        }
        // 6. Audio de Respawn
        //if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerRespawn");

        // 7. Peque�a pausa en negro
        yield return new WaitForSeconds(0.2f);

        // 8. Fade In (Pantalla visible)
        if (screenFader) yield return screenFader.FadeIn();

        // 9. Desactivar el Fader para limpiar el editor
        if (screenFader) screenFader.gameObject.SetActive(false);


    }

}
