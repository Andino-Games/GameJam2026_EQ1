using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Diana : MonoBehaviour
{
    public void ExitGame()
    {
        // Application.Quit();
        SceneManager.LoadScene("Menu");
    }
}
