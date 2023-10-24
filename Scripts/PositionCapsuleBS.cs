using Mirror;
using TMPro;
using UnityEngine;

public class PositionCapsuleBS : MonoBehaviour
{
    public Color playerColor;
    public int playerId;
    [SerializeField] CurtainsBS curtains;

    private void Start()
    {
        playerColor = NetworkStartupBS.Instance.playerColors[playerId - 1];
        transform.Find("Text").GetComponent<TextMeshPro>().color = playerColor;
    }

    public void OnTriggerEnter(Collider other)
    {
        NetworkManager.singleton.StartClient();
        gameObject.tag = "ActivePosCap";
        curtains.SetOpen(true);
    }

    public void OnTriggerExit(Collider other)
    {
        NetworkManager.singleton.StopClient();
        gameObject.tag = "Untagged";
        curtains.SetOpen(false);
    }

    public void GetInfos(PlayerObjectBS playerObject)
    {
        playerObject.SetInfos(transform.position.x, playerColor, playerId);
    }
}
