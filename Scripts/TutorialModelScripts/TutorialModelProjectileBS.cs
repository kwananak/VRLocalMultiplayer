using UnityEngine;

public class TutorialModelProjectileBS : MonoBehaviour
{
    public Type type;
    public Color color;
    [SerializeField] int colorInt;

    private void Start()
    {
        color = NetworkStartupBS.Instance.playerColors[colorInt];
        GetComponent<Renderer>().material.color = color;
    }
}
