using UnityEngine;
using Mirror;

public class TargetBS : NetworkBehaviour
{
    public int playerControl { get; private set; }
    bool up;
    float step;
    [SerializeField] GameObject[] flames;
    [SerializeField] GameObject[] hitPrefabs;
    [SerializeField] Transform[] outerMeshes;
    [SerializeField] Renderer[] childRenderers;
    [SyncVar] float rotationSpeed;
    [SyncVar] Vector3 rotateDirection;
    [SyncVar(hook = nameof(SetColor))] Color color;
    [SyncVar(hook = nameof(SetType))] public Type type;

    private void Start()
    {
        if (!isServer) return;
        step = Random.Range(0.5f, 1.5f);
        if (Random.value > 0.5f) up = true; 
        if (Random.value > 0.5f) rotateDirection = Vector3.up; else rotateDirection = Vector3.down;
        rotationSpeed = Random.Range(0.5f, 1.0f) * 50.0f;
    }

    private void Update()
    {
        if (!isServer) return;
        if (GameManagerBS.Instance.movingTargets) MovingTarget();
    }

    private void FixedUpdate()
    {
        if (GameManagerBS.Instance.spinningTargets) SpinningTarget();
    }

    void SpinningTarget()
    {
        outerMeshes[0].Rotate(rotateDirection * rotationSpeed * Time.deltaTime);
        outerMeshes[1].Rotate(-rotateDirection * rotationSpeed * Time.deltaTime);
        outerMeshes[2].Rotate(rotateDirection * rotationSpeed * Time.deltaTime);
    }

    void MovingTarget()
    {
        if (up) transform.position += new Vector3(0.0f, step * Time.deltaTime, 0.0f); else transform.position -= new Vector3(0.0f, step * Time.deltaTime, 0.0f);
        if (transform.position.y > 6.0f) up = false;
        if (transform.position.y < 2.0f) up = true;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (!isServer) return;
        ProjectileBS proj = collision.GetComponent<ProjectileBS>();
        switch (proj.projectileType)
        {
            case Type.Fire:
                {
                    ExplosionSpawn(0, proj.transform.position);
                    break;
                }
            case Type.Ice:
                {
                    ExplosionSpawn(1, proj.transform.position);
                    break;
                }
            case Type.Water:
                {
                    ExplosionSpawn(2, proj.transform.position);
                    break;
                }
            default:
                break;
        }
        if (GameManagerBS.Instance.gameStarted) TargetHit(proj);
    }

    [Server]
    void ExplosionSpawn(int explo, Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(hitPrefabs[explo], pos, Quaternion.identity));
    }

    [Server]
    void TargetHit(ProjectileBS passedCollision)
    {
        //Debug.Log($"targethit playercontrol: {playerControl} projectile playerID: {passedCollision.playerId}");
        if (playerControl == passedCollision.playerId)
        {
            NetworkServer.Destroy(passedCollision.gameObject);
            return;
        }
        if (type == Type.None)
        {
            type = passedCollision.projectileType;
            playerControl = passedCollision.playerId;
            foreach(Renderer renderer in childRenderers) renderer.material.color = passedCollision.color;
            color = passedCollision.color;
        }
        else if ((type == Type.Fire && passedCollision.projectileType == Type.Water) 
                || (type == Type.Ice && passedCollision.projectileType == Type.Fire) 
                || (type == Type.Water && passedCollision.projectileType == Type.Ice && GameManagerBS.Instance.hardmode) 
                || (type == Type.Water && passedCollision.projectileType == Type.Fire && !GameManagerBS.Instance.hardmode))
        {
            type = Type.None;
            playerControl = 0;
            foreach (Renderer renderer in childRenderers) renderer.material.color = Color.gray;
            color = Color.gray;
        }
        NetworkServer.Destroy(passedCollision.gameObject);
        ClientRpcDestroyProjectile(passedCollision.gameObject);
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

    [ClientRpc]
    void ClientRpcDestroyProjectile(GameObject go)
    {
        Destroy(go);
    }

    void SetColor(Color oldColor, Color newColor)
    {
        foreach (Renderer renderer in childRenderers) renderer.material.color = color;
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
