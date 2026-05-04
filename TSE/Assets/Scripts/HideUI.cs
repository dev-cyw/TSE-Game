using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hide : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button playBTN;

    public void HideUI()
    {
        inputField.gameObject.SetActive(!inputField.gameObject.activeSelf);
        playBTN.gameObject.SetActive(!playBTN.gameObject.activeSelf);
    }
}
