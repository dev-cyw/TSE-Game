using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorNextLevel : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)  
    { 
        if(collision.gameObject.CompareTag("Player"))   //check if the player collides with the door
        {
            NextLevel();    //if player does load the next level
        }
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //load the next level in the build
    }
}

