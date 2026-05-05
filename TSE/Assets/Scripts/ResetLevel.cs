using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void ResetPlayer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload the current scene
    }
}
