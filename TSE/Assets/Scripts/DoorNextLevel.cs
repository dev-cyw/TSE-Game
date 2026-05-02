using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorNextLevel : MonoBehaviour
{
    [SerializeField] Animator transition;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   //check if the player collides with the door
        {
            NextLevel();    //if player does load the next level
        }
    }
    private void NextLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //load the next level in the build
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        transition.SetTrigger("End");   //play the transition animation
        yield return new WaitForSeconds(1);  //wait for the animation to finish
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);   //load the next level in the build
        transition.SetTrigger("Start");     //play the end transition animation
    }
}

