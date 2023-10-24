using UnityEngine;

public class ImaginePositionCapsuleBS : PositionCapsuleBS
{
    [SerializeField] Renderer markerRenderer;

    private void Start()
    {
        playerColor = NetworkStartupBS.Instance.playerColors[playerId - 1];
        markerRenderer.material.color = playerColor;
    }
}
