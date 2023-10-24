using UnityEngine;

public class CurtainsBS : MonoBehaviour
{
    bool openCurtains = false;
    [SerializeField] GameObject[] curtains;
    Vector3 leftTargetRotation = new Vector3(0.0f, 0.0f, -90.0f);
    Vector3 rightTargetRotation = new Vector3(0.0f, 0.0f, 90.0f);
    float step = 0.5f;
    float limit = 70.0f;

    void Update()
    {
        if (openCurtains)
        {
            if (leftTargetRotation.y > -limit) curtains[0].transform.rotation = Quaternion.Euler(leftTargetRotation += new Vector3(0.0f, -step, 0.0f));
            if (rightTargetRotation.y < limit) curtains[1].transform.rotation = Quaternion.Euler(rightTargetRotation += new Vector3(0.0f, step, 0.0f));
        }
        else
        {
            if (leftTargetRotation.y < 0.0f) curtains[0].transform.rotation = Quaternion.Euler(leftTargetRotation += new Vector3(0.0f, step, 0.0f));
            if (rightTargetRotation.y > 0.0f) curtains[1].transform.rotation = Quaternion.Euler(rightTargetRotation += new Vector3(0.0f, -step, 0.0f));
        } 
    }

    public void SetOpen(bool b)
    {
        openCurtains = b;
    }
}
