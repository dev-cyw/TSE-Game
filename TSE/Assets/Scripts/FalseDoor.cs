using UnityEngine;

public class FalseDoor : MonoBehaviour
{
    public Player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))   //check if the player collides with the door
        {
            player.Die();  
        }
    }
}
