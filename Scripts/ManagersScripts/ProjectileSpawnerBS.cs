using UnityEngine;
using Mirror;
using System.Collections;

public class ProjectileSpawnerBS : NetworkBehaviour
{
    [SerializeField] GameObject projectilePrefab;

    void Start()
    {
        if (isServer && !GameManagerBS.Instance.gameStarted) StartCoroutine(DelayedSpawn()); else StartCoroutine(DelayedGameSpawns(GameManagerBS.Instance.hardmode));
    }

    [Server]
    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnProjectile(transform.position + new Vector3(0.0f, 1.1f, 0.5f), Type.Fire);
    }

    [Server]
    IEnumerator DelayedGameSpawns(bool hard)
    {
        yield return new WaitForSeconds(0.5f);
        if (hard) GameSpawnsHard(); else GameSpawnsEasy();
    }

    [Server]
    public void SpawnProjectile(Vector3 spawnPos, Type passedType)
    {
        GameObject go = Instantiate(projectilePrefab);
        NetworkServer.Spawn(go, connectionToClient);
        go.GetComponent<ProjectileBS>().Setup(spawnPos, gameObject, passedType);
    }

    [Server]
    public void GameSpawnsHard()
    {
        SpawnProjectile(transform.position + new Vector3(-0.3f, 1.1f, 0.4f), Type.Fire);
        SpawnProjectile(transform.position + new Vector3(0.0f, 1.1f, 0.5f), Type.Ice);
        SpawnProjectile(transform.position + new Vector3(0.3f, 1.1f, 0.4f), Type.Water);
    }

    [Server]
    public void GameSpawnsEasy()
    {
        SpawnProjectile(transform.position + new Vector3(-0.2f, 1.1f, 0.5f), Type.Fire);
        SpawnProjectile(transform.position + new Vector3(0.2f, 1.1f, 0.5f), Type.Water);
    }
}
