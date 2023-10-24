using UnityEngine;

public class SkyboxBS : MonoBehaviour
{
    [SerializeField] float speed = 0.5f;

    void Update()
    {
        float adjustedSpeed = speed * Time.deltaTime;
        transform.Rotate(Vector2.down * adjustedSpeed);
    }
}
