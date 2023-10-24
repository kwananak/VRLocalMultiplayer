using Mirror;
using TMPro;
using UnityEngine;

public class UIBS : NetworkBehaviour
{
    TextMeshPro uiText;
    [SerializeField] int positionInt;
    [SyncVar(hook = nameof(UpdateText))] string syncedText = "";

    void Start()
    {
        uiText = GetComponent<TextMeshPro>();
        uiText.color = NetworkStartupBS.Instance.playerColors[positionInt];
        UpdateText("", "");
    }

    [Server]
    public void SetSyncedText(string text)
    {
        syncedText = text;
    }

    void UpdateText(string oldText, string newText)
    {
        uiText.text = syncedText;
    }

    public string GetText()
    {
        return syncedText;
    }
}
