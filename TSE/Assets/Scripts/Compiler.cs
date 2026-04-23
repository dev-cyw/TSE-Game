using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Compiler : MonoBehaviour
{
    public TMP_InputField inputField;

    /*
     * Valid Commands
     * - move_right(10)
     * - Move Left
     * - Jump ?
     * - 
     * 
     */

    /*
     * 
     * 
     * 
     */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        print(inputField.text);
    }
}