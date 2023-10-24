using Mirror;
using System.Collections;
using UnityEngine;

public class ExplosionsBS : NetworkBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyTimer());
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(2);
        NetworkServer.Destroy(gameObject);
    }
}
