using TMPro;
using UnityEngine;

public class TutorialModelBS : MonoBehaviour
{
    [SerializeField] float size = 0.015f;
    [SerializeField] Transform handAnchor;
    [SerializeField] GameObject easyModel;

    void FixedUpdate()
    {
        transform.position = handAnchor.position;
        transform.rotation = handAnchor.rotation;
        if (GameManagerBS.Instance != null)
        {
            if (GameManagerBS.Instance.hardmode) {
                        if (easyModel.activeSelf) easyModel.SetActive(false);
                        return;
                    }
        }
        if (transform.forward.x > 0) easyModel.SetActive(false); else easyModel.SetActive(true);
        transform.localScale = new Vector3(size, size, size) * -transform.forward.x;
    }
}
