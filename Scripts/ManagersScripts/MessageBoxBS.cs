using TMPro;
using UnityEngine;

public class MessageBoxBS : MonoBehaviour
{
    public static MessageBoxBS Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    public void Log(string message)
    {
        gameObject.GetComponent<TextMeshPro>().text += message + "\n";
    }

    public void SingleLog(string message)
    {
        gameObject.GetComponent<TextMeshPro>().text = message;
    }
}
