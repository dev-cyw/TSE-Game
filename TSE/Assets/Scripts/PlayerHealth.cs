using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public int health = 100;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Spike"))    //check if player collides with the spikes
        {
            health -= 100;

            if(health <= 0)
            {
                Die();
            }
        }
        else if(collision.gameObject.CompareTag("FalseDoor"))   //check if the player collides with the false door
        {
            health -= 100;
            {
                if (health <= 0)
                {
                    Die();
                }
            }
        }
        else if (collision.gameObject.CompareTag("CrouchTrap")) //check if the player collides with the crouch trap
        {
            health -= 25;
            {
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    public void Die()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
