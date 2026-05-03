using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMenu : MonoBehaviour
{
    public void Menu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); // Load the main menu scene
        Time.timeScale = 1; // Resume the game
    }
}
