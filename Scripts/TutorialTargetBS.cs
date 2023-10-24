using UnityEngine;
using Mirror;

public class TutorialTargetBS : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetType))] Type type;
    [SerializeField] GameObject[] flames;
    [SerializeField] GameObject[] hitPrefabs;
    [SerializeField] float rotationSpeed = 10.0f;
    [SyncVar] Vector3 rotateDirection;
    [SyncVar(hook = nameof(SetColor))] Color syncedColor;
    [SerializeField] Renderer[] renderers;
    [SerializeField] Transform[] outerMeshes;
    ProjectileSpawnerBS projectileSpawner;
    int playerId;

    private void Start()
    {
        if (isServer) if (Random.value > 0.5) rotateDirection = Vector3.up; else rotateDirection = Vector3.down;
    }

    private void Update()
    {
        outerMeshes[0].Rotate(rotationSpeed * Time.deltaTime * rotateDirection);
        outerMeshes[1].Rotate(rotationSpeed * Time.deltaTime * -rotateDirection);
        outerMeshes[2].Rotate(rotationSpeed * Time.deltaTime * rotateDirection);
    }

    [Server]
    public void Setup(ProjectileSpawnerBS spawner, int id)
    {
        projectileSpawner = spawner;
        playerId = id;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (!isOwned || !collision.GetComponent<ProjectileBS>().isOwned) return;
        if (GameManagerBS.Instance.hardmode) HardCheck(collision); else EasyCheck(collision);
    }

    void HardCheck(Collider collision)
    {
        ProjectileBS projectile = collision.GetComponent<ProjectileBS>();
        if (type == Type.None && projectile.projectileType == Type.Fire)
        {
            ExplosionSpawn(0, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.Fire);
            CommandColorChange(collision.GetComponent<ProjectileBS>().color);
            SpawnNewProj(Type.Water);
        }
        else if (type == Type.Fire && projectile.projectileType == Type.Water)
        {
            ExplosionSpawn(2, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.None);
            CommandColorChange(Color.gray);
            SpawnNewProj(Type.Ice);
        }
        else if (type == Type.None && projectile.projectileType == Type.Ice)
        {
            ExplosionSpawn(1, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.Ice);
            CommandColorChange(collision.GetComponent<ProjectileBS>().color);
            SpawnNewProj(Type.Fire);
        }
        else if (type == Type.Ice && projectile.projectileType == Type.Fire)
        {
            ExplosionSpawn(0, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            PlayerReady();
        }
    }

    void EasyCheck(Collider collision)
    {
        ProjectileBS projectile = collision.GetComponent<ProjectileBS>();
        if (type == Type.None && projectile.projectileType == Type.Fire)
        {
            ExplosionSpawn(0, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.Fire);
            CommandColorChange(collision.GetComponent<ProjectileBS>().color);
            SpawnNewProj(Type.Water);
        }
        else if (type == Type.Fire && projectile.projectileType == Type.Water)
        {
            ExplosionSpawn(2, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.None);
            CommandColorChange(Color.gray);
            SpawnNewProj(Type.Water);
        }
        else if (type == Type.None && projectile.projectileType == Type.Water)
        {
            ExplosionSpawn(2, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            ChangeType(Type.Water);
            CommandColorChange(collision.GetComponent<ProjectileBS>().color);
            SpawnNewProj(Type.Fire);
        }
        else if (type == Type.Water && projectile.projectileType == Type.Fire)
        {
            ExplosionSpawn(0, collision.transform.position);
            foreach (ProjectileBS proj in FindObjectsOfType<ProjectileBS>()) if (proj.isOwned) proj.CommandDestroy();
            NetworkServer.Destroy(gameObject);
            PlayerReady();
        }

    }

    [Command]
    void CommandColorChange(Color passedColor)
    {
        syncedColor = passedColor;
    }


    [Command]
    void ExplosionSpawn(int explo, Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(hitPrefabs[explo], pos, Quaternion.identity));
    }

    [Command]
    void ChangeType(Type passedType)
    {
        type = passedType;
    }

    [Command]
    void SpawnNewProj(Type type)
    {
        projectileSpawner.SpawnProjectile(projectileSpawner.transform.position + new Vector3(0.0f, 1.1f, 0.5f), type);
    }

    [Command]
    void PlayerReady()
    {
        Destroy(gameObject);
        RemoveStartTarget();
        NetworkServer.Destroy(gameObject);
        GameManagerBS.Instance.AddReadyPlayer(playerId);
    }

    [ClientRpc]
    void RemoveStartTarget()
    {
        Destroy(gameObject);
    }

    void SetColor(Color oldColor, Color newColor)
    {
        foreach (Renderer renderer in renderers) renderer.material.color = syncedColor;
    }

    void SetType(Type oldType, Type newType)
    {
        switch (type)
        {
            case Type.None:
                {
                    flames[0].SetActive(false);
                    flames[1].SetActive(false);
                    flames[2].SetActive(false);
                    break;
                }
            case Type.Fire:
                {
                    flames[0].SetActive(true);
                    flames[1].SetActive(false);
                    flames[2].SetActive(false);
                    break;
                }
            case Type.Ice:
                {
                    flames[0].SetActive(false);
                    flames[1].SetActive(true);
                    flames[2].SetActive(false);
                    break;
                }
            case Type.Water:
                {
                    flames[0].SetActive(false);
                    flames[1].SetActive(false);
                    flames[2].SetActive(true);
                    break;
                }
            default:
                return;
        }
    }
}
