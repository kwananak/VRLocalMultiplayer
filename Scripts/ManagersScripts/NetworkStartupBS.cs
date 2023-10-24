using UnityEngine;
using Mirror;

public class NetworkStartupBS : MonoBehaviour
{
    public static NetworkStartupBS Instance { get; private set; }
    [SerializeField] bool server;
    [SerializeField] GameObject serverSetup, clientSetup, xrRig;
    public Color[] playerColors;

    private void Start()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;

        if (server)
        {
            NetworkManager.singleton.StartServer();
            Instantiate(serverSetup);
        }
        else
        {
            Instantiate(xrRig);
            Instantiate(clientSetup);
        }
    }
}
