using UnityEngine;

public class CrouchTrap : MonoBehaviour
{
    public Player player;   //inherit player 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   //check if the player collides with the crouch trap
        {
            player.Die();   //call players die function
        }
    }
}

