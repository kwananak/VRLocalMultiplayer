using Mirror;
using UnityEngine;
using System.Collections;

public class TutorialTargetSpawnerBS : NetworkBehaviour
{
    [SerializeField] GameObject tutorialTargetPrefab;
    ProjectileSpawnerBS projectileSpawner;

    void Start()
    {
        if (isServer && !GameManagerBS.Instance.gameStarted)
        {
            projectileSpawner = transform.parent.Find("ProjectileSpawner").GetComponent<ProjectileSpawnerBS>();
            StartCoroutine(DelayedSpawn());
        }
    }

    [Server]
    IEnumerator DelayedSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        SpawnProjectile();
    }

    [Server]
    public void SpawnProjectile()
    {
        GameObject go = Instantiate(tutorialTargetPrefab, transform.position + new Vector3(0.0f, 1.0f, 3.0f), Quaternion.identity);
        NetworkServer.Spawn(go, connectionToClient);
        go.GetComponent<TutorialTargetBS>().Setup(projectileSpawner, transform.parent.GetComponent<PlayerObjectBS>().playerId);
    }
}
