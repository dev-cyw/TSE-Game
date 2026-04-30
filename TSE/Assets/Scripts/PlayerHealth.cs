using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Player : MonoBehaviour
{
    public int health = 100;
    private Animator anim;
    private Rigidbody2D rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Spike"))    //check if player collides with the spikes
        {
            health -= 100;  //kill player

            if(health <= 0)
            {
                Die();
            }
        }
        else if(collision.gameObject.CompareTag("FalseDoor"))   //check if the player collides with the false door
        {
            health -= 100;  //kill player
            {
                if (health <= 0)
                {
                    Die();
                }
            }
        }
        else if (collision.gameObject.CompareTag("CrouchTrap")) //check if the player collides with the crouch trap
        {
            health -= 25;   //damage the player
            {
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }

    private void Die()
    {
        //rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("Death");
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

