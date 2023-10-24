using TMPro;
using UnityEngine;

public class PlayerColliderBS : MonoBehaviour
{
    [SerializeField] Transform leader;
    public Transform targetingSystem;

    void Update()
    {
        if (!OVRManager.isHmdPresent) return;
        transform.position = Vector3.MoveTowards(transform.position, leader.position, Time.deltaTime);
        targetingSystem.rotation = leader.rotation;
    }

    /*void FixedUpdate()
    {
        int layerMask = 1 << 10;
        RaycastHit hit;
        if (Physics.Raycast(targetingSystem.position, targetingSystem.TransformDirection(Vector3.forward), out hit, 10.0f, layerMask, QueryTriggerInteraction.Collide))
        {
            MessageBoxBS.Instance.Log("Did Hit " +  hit.collider.name);
        }
    }*/
}
