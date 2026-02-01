using UnityEngine;

public class ChangeLevel : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    private GameObject player;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.transform.position = playerPosition.position;
            
        }
    }
}
