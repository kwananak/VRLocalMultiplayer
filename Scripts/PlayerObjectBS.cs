using UnityEngine;
using Mirror;

public class PlayerObjectBS : NetworkBehaviour
{
    [SyncVar] public Color syncedColor;
    [SyncVar] public int playerId;
    [SyncVar(hook = nameof(SetPosition))] Vector3 syncedPosition;

    private void Start()
    {
        if (isOwned) GetInfosFromCap();
    }

    void GetInfosFromCap()
    {
        GameObject.FindGameObjectWithTag("ActivePosCap").GetComponent<PositionCapsuleBS>().GetInfos(this);
    }

    [Command]
    public void SetInfos(float xPos, Color passedColor, int passedId)
    {
        syncedPosition = new Vector3(xPos, 0.0f, 1.0f);
        transform.position = syncedPosition;
        syncedColor = passedColor;
        playerId = passedId;
    }

    void SetPosition(Vector3 oldPos, Vector3 newPos)
    {
        transform.position = syncedPosition;
    }
}
