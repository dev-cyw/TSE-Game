using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level 1"); // Reload the current scene
        Time.timeScale = 1; // Resume the game
    }
}
