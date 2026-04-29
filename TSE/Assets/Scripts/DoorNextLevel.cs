using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorNextLevel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.CompareTag("Player"))
        {
            NextLevel();
        }
    }

    private void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

