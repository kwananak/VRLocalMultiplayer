using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerBS : NetworkBehaviour
{
    public static GameManagerBS Instance { get; private set; }
    public bool countdownStarted { get; private set; }
    [SyncVar] public bool gameStarted;
    int connectedPlayers;
    int readyPlayers;
    int[] scores;
    bool[] playersOn = new bool[] { false, false, false, false };
    GameObject[] targets;
    [SerializeField] UIBS[] textScores;
    [SerializeField] UIBS timer;
    [SerializeField] int gameLength = 10;
    [SerializeField] int countdownLength = 3;
    [SyncVar] public bool hardmode = false;
    [SyncVar] public bool movingTargets = true;
    [SyncVar] public bool spinningTargets = true;
    [SyncVar(hook = nameof(SetPlayersMusic))] bool playersMusic = true;
    [SyncVar(hook = nameof(SetCrosshair))] bool crosshair;

    private void Start()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    private void Update()
    {
        if (!isServer) return;
        if (gameStarted)
        {
            UpdateScore();
            if (connectedPlayers == 0) EndGame();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !countdownStarted)
        {
            StartCoroutine(CountdownToStart());
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            crosshair = !crosshair;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            spinningTargets = !spinningTargets;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            movingTargets = !movingTargets;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            playersMusic = !playersMusic;
        }
    }

    [Server]
    void UpdateScore()
    {
        int[] tempScores = new int[4];
        foreach (GameObject target in targets) if (target.GetComponent<TargetBS>().playerControl > 0) tempScores[target.GetComponent<TargetBS>().playerControl - 1]++;
        scores = tempScores;
        for (int i = 0; i < scores.Length; i++)
        {
            if (!playersOn[i]) textScores[i].SetSyncedText("");
            else textScores[i].SetSyncedText(scores[i].ToString());
        }
    }

    [Server]
    public void AddPlayer()
    {
        connectedPlayers++;
    }

    [Server]
    public void RemovePlayer()
    {
        connectedPlayers--;
        if (connectedPlayers == 0 && !gameStarted) EmptyGameCleanUp();
    }

    [Server]
    public void AddReadyPlayer(int playerId)
    {
        readyPlayers++;
        textScores[playerId - 1].SetSyncedText("prêt");
    }

    [Server]
    void SetActivePlayers()
    {
        PlayerObjectBS[] activePlayers = FindObjectsOfType<PlayerObjectBS>();
        foreach (PlayerObjectBS activePlayer in activePlayers)
        {
            playersOn[activePlayer.playerId - 1] = true;
            if (hardmode) activePlayer.transform.Find("ProjectileSpawner").GetComponent<ProjectileSpawnerBS>().GameSpawnsHard(); else activePlayer.transform.Find("ProjectileSpawner").GetComponent<ProjectileSpawnerBS>().GameSpawnsEasy();
        }
    }

    [Server]
    IEnumerator CountdownToStart()
    {
        EntitiesCleanUp();
        countdownStarted = true;
        MusicFadeLow();
        ClientRpcTimerColor(Color.red);
        for (int i = countdownLength; i > 0; i--) 
        {
            timer.SetSyncedText(i.ToString());
            yield return new WaitForSeconds(1);
        }
        StartCoroutine(StartGame());
    }

    [Server]
    IEnumerator StartGame()
    {
        int localTimer = gameLength;
        targets = TargetSpawnerBS.Instance.SpawnTargets();
        SetActivePlayers();
        gameStarted = true;
        ClientRpcTimerColor(Color.white);
        MusicStartHype();
        while (localTimer >= 0 && gameStarted)
        {
            timer.SetSyncedText(localTimer.ToString());
            yield return new WaitForSeconds(1.0f);
            localTimer--;
        }
        EndGame();
    }

    [Server]
    void EndGame()
    {
        MusicFadeHype();
        gameStarted = false;
        countdownStarted = false;
        timer.SetSyncedText("Partie terminée");
        DeclareWinners(CheckWinners());
        if (connectedPlayers == 0) EmptyGameCleanUp();
    }

    [Server]
    List<int> CheckWinners()
    {
        List<int> winners = new List<int>();
        int winnerScore = 0;
        for (int i = 0; i < scores.Length; i++)
        {
            if (scores[i] > winnerScore)
            {
                winners = new List<int> { i };
                winnerScore = scores[i];
            }
            else if (scores[i] == winnerScore)
            {
                winners.Add(i);
            }
            textScores[i].SetSyncedText("");
        }
        return winners;
    }

    [Server]
    void DeclareWinners(List<int> winners)
    {
        foreach (int winner in winners)
        {
            if (playersOn[winner]) textScores[winner].SetSyncedText("Victoire!");
        }
    }

    [Server]
    void EmptyGameCleanUp()
    {
        timer.SetSyncedText("");
        foreach (UIBS textScore in textScores) textScore.SetSyncedText("");
        EntitiesCleanUp();
        readyPlayers = 0;
        playersOn = new bool[] { false, false, false, false };
    }

    [Server]
    void EntitiesCleanUp()
    {
        foreach (TargetBS t in FindObjectsOfType<TargetBS>()) NetworkServer.Destroy(t.gameObject);
        foreach (TutorialTargetBS tt in FindObjectsOfType<TutorialTargetBS>()) NetworkServer.Destroy(tt.gameObject);
        foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) NetworkServer.Destroy(proj.gameObject);
    }

    [Server]
    void MusicStartHype()
    {
        MusicManagerBS.Instance.StartHype();
        ClientRpcMusicStartHype();
    }

    [ClientRpc]
    void ClientRpcMusicStartHype()
    {
        MusicManagerBS.Instance.StartHype();
    }
    
    [Server]
    void MusicFadeLow()
    {
        StartCoroutine(MusicManagerBS.Instance.FadeOutLow());
        ClientRpcMusicFadeLow();
    }

    [ClientRpc]
    void ClientRpcMusicFadeLow()
    {
        StartCoroutine(MusicManagerBS.Instance.FadeOutLow());
    }

    [Server]
    void MusicFadeHype()
    {
        StartCoroutine(MusicManagerBS.Instance.FadeOutHype());
        ClientRpcMusicFadeHype();
    }

    [ClientRpc]
    void ClientRpcMusicFadeHype()
    {
        StartCoroutine(MusicManagerBS.Instance.FadeOutHype());
    }

    [ClientRpc]
    void ClientRpcTimerColor(Color color)
    {
        timer.GetComponent<TextMeshPro>().color = color;
    }

    void SetCrosshair(bool oldCrosshair, bool newCrosshair)
    {
        FindAnyObjectByType<CrosshairBS>().SetCrosshair(crosshair);
    }

    void SetPlayersMusic(bool oldMusic, bool newMusic)
    {
        MusicManagerBS.Instance.GetComponent<AudioSource>().mute = playersMusic;
    }
}
