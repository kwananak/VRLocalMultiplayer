using UnityEngine;

public class ProjectionSurfaceBS : MonoBehaviour
{
    public OVRPassthroughLayer passthrough;

    private void Start()
    {
        passthrough.AddSurfaceGeometry(gameObject, false);
        GetComponent<MeshRenderer>().enabled = false;
    }
}
