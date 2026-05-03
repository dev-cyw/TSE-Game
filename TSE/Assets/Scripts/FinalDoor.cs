using UnityEngine;

public class FinalDoor : MonoBehaviour
{
    public GameObject FinalDoorUI;

    private void OnTriggerEnter2D(Collider2D collision )
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0; // Pause the game
            FinalDoorUI.SetActive(true); // Show the UI
        }
    }
}