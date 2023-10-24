using Mirror.Examples.Tanks;
using UnityEngine;

public class EasyTutorialModelBS : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Transform[] projectiles;
    [SerializeField] Transform[] projectilesVert;
    [SerializeField] float speed = 100.0f;

    void Update()
    {
        float adjustedSpeed = speed * Time.deltaTime;
        foreach (Transform projectile in projectiles) projectile.Rotate(Vector2.down * adjustedSpeed);
        foreach (Transform projectile in projectilesVert) projectile.Rotate(Vector2.left * adjustedSpeed * 1.5f);
        target.Rotate(Vector2.up * adjustedSpeed * 0.8f);
    }
}
