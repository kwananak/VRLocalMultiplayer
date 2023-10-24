using UnityEngine;

public class CrosshairBS : MonoBehaviour
{
    [SerializeField] GameObject point;

    public void SetCrosshair(bool set)
    {
        point.SetActive(set);
    }
}
