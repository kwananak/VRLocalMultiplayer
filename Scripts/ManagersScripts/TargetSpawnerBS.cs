using UnityEngine;
using Mirror;

public class TargetSpawnerBS : NetworkBehaviour
{
    public static TargetSpawnerBS Instance { get; private set; }
    [SerializeField] int numberOfTargets = 5;
    [SerializeField] GameObject targetprefab;

    private void Start()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
    }

    public GameObject[] SpawnTargets()
    {
        float spread = 16.0f / (numberOfTargets - 1.0f);
        GameObject[] targets = new GameObject[numberOfTargets];
        for (int i = 0; i < numberOfTargets; i++)
        {
            targets[i] = Instantiate(targetprefab, new Vector3((i * spread) - 8, 3.0f + Random.Range(0, 2), 15.0f), Quaternion.identity);
            NetworkServer.Spawn(targets[i]);
        }
        return targets;
    }
}
