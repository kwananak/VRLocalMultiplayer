using UnityEngine;

public class TutorialModelTargetBS : MonoBehaviour
{
    Type type = Type.None;
    [SerializeField] GameObject[] flames;
    [SerializeField] GameObject[] explosions;
    [SerializeField] Renderer targetMesh;
    [SerializeField] Transform[] outerMeshes;

    private void Update()
    {
        outerMeshes[0].Rotate(Vector3.up * 10.0f * Time.deltaTime);
        outerMeshes[1].Rotate(-Vector3.up * 10.0f * Time.deltaTime);
        outerMeshes[2].Rotate(Vector3.up * 10.0f * Time.deltaTime);
    }

    public void OnTriggerEnter(Collider other)
    {
        TutorialModelProjectileBS proj = other.GetComponent<TutorialModelProjectileBS>();
        switch (proj.type)
        {
            case Type.Fire:
                {
                    Instantiate(explosions[0], proj.transform.position, Quaternion.identity);
                    break;
                }
            case Type.Water:
                {
                    Instantiate(explosions[1], proj.transform.position, Quaternion.identity);
                    break;
                }
            default:
                break;
        }
        if (type == Type.None)
        {
            type = proj.type;
            targetMesh.material.color = proj.color;
        }
        else if ((type == Type.Fire && proj.type == Type.Water) || (type == Type.Water && proj.type == Type.Fire))
        {
            type = Type.None;
            targetMesh.material.color = Color.white;
        }
        switch (type)
        {
            case Type.None:
                {
                    flames[0].SetActive(false);
                    flames[1].SetActive(false);
                    break;
                }
            case Type.Fire:
                {
                    flames[0].SetActive(true);
                    flames[1].SetActive(false);
                    break;
                }
            case Type.Water:
                {
                    flames[0].SetActive(false);
                    flames[1].SetActive(true);
                    break;
                }
            default:
                return;
        }
    }
}
