using Script.UI;
using System.Collections;
using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    [SerializeField] private ScreenFader screenFader;

    private GameObject player;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        player.transform.position = playerPosition.position;
        // 6. Audio de Respawn
        //if (AudioManager.Instance != null) AudioManager.Instance.Play("PlayerRespawn");

        // 7. Pequeña pausa en negro
        yield return new WaitForSeconds(0.2f);

        // 8. Fade In (Pantalla visible)
        if (screenFader) yield return screenFader.FadeIn();

        // 9. Desactivar el Fader para limpiar el editor
        if (screenFader) screenFader.gameObject.SetActive(false);


    }

}
