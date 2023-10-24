using Mirror;
using UnityEngine;

public class NetworkManagerBS : NetworkManager
{
    public override void OnServerConnect(NetworkConnectionToClient conn)
    {
        base.OnServerConnect(conn);
        GameManagerBS.Instance.AddPlayer();
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        GameManagerBS.Instance.RemovePlayer();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if (MusicManagerBS.Instance.GetComponent<AudioSource>().clip.name == "HypeTrack") StartCoroutine(MusicManagerBS.Instance.FadeOutHype(1));
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (GameManagerBS.Instance.gameStarted) MusicManagerBS.Instance.StartHype();
    }
}
