using UnityEngine;
using Mirror;

public class ProjectileBS : NetworkBehaviour
{
    float respawnThreshold = 5.0f;
    float destroyThreshold = 100.0f;
    bool respawned = false;
    bool destroyed = false;
    ProjectileSpawnerBS projectileSpawner;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] GameObject[] flames;
    [SerializeField] float forceMult = 4.0f;
    [SerializeField] float targetCorrection = 0.2f;
    [SyncVar(hook = nameof(SetColor))] public Color color;
    [SyncVar(hook = nameof(SetPosition))] Vector3 syncedPosition;
    [SyncVar] public int playerId;
    [SyncVar(hook = nameof(SetType))] public Type projectileType;

    void FixedUpdate()
    {
        if (!isOwned) return;
        if (transform.position.x < -destroyThreshold || transform.position.x > destroyThreshold ||
            transform.position.y < -destroyThreshold || transform.position.y > destroyThreshold ||
            transform.position.z < -destroyThreshold || transform.position.z > destroyThreshold)
        {
            if (destroyed) return;
            destroyed = true;
            CommandDestroy();
        }
        if (respawned) return;
        if (transform.position.x < -respawnThreshold || transform.position.x > respawnThreshold ||
            transform.position.y < -respawnThreshold || transform.position.y > respawnThreshold ||
            transform.position.z < -respawnThreshold || transform.position.z > respawnThreshold)
        {
            respawned = true;
            Respawn();
        }
    }

    [Server]
    public void Setup(Vector3 passedPos, GameObject passedSpawner, Type passedType)
    {
        transform.position = passedPos;
        syncedPosition = transform.position;
        projectileSpawner = passedSpawner.GetComponent<ProjectileSpawnerBS>();
        GetComponent<Renderer>().material.color = passedSpawner.GetComponentInParent<PlayerObjectBS>().syncedColor;
        color = GetComponent<Renderer>().material.color;
        playerId = passedSpawner.GetComponentInParent<PlayerObjectBS>().playerId;
        projectileType = passedType;
        switch (projectileType)
        {
            case Type.Fire:
                {
                    flames[0].SetActive(true);
                    break;
                }
            case Type.Ice:
                {
                    flames[1].SetActive(true);
                    break;
                }
            case Type.Water:
                {
                    flames[2].SetActive(true);
                    break;
                }
            default:
                return;
        }
    }

    [Command]
    void Respawn()
    {
        projectileSpawner.SpawnProjectile(syncedPosition, projectileType);
    }

    [Command]
    public void CommandDestroy()
    {
        NetworkServer.Destroy(gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isOwned || other.transform.parent.name != "GripPoint") return;
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.velocity != Vector3.zero && rb.velocity.z < forceMult * 10)
        {
            RaycastHit hit;
            Transform targeting = FindFirstObjectByType<PlayerColliderBS>().targetingSystem;
            if (Physics.Raycast(targeting.position, targeting.TransformDirection(Vector3.forward), out hit, 20.0f, 1 << 10, QueryTriggerInteraction.Collide))
            {
                rb.velocity += ((hit.collider.transform.position - transform.position) * targetCorrection);
                rb.velocity *= forceMult * (1.0f - targetCorrection);
            }
            else
            {
               rb.velocity *= forceMult;
            }
            rb.useGravity = true;
        }
    }

    void SetColor(Color oldColor, Color newColor)
    {
        GetComponent<Renderer>().material.color = color;
    }

    void SetPosition(Vector3 oldPos, Vector3 newPos)
    {
        transform.position = syncedPosition;
    }

    void SetType(Type oldType, Type newType)
    {
        switch (projectileType)
        {
            case Type.Fire:
                {
                    flames[0].SetActive(true);
                    break;
                }
            case Type.Ice:
                {
                    flames[1].SetActive(true);
                    break;
                }
            case Type.Water:
                {
                    flames[2].SetActive(true);
                    break;
                }
            default:
                return;
        }
    }
}
