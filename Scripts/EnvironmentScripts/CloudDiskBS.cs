using System.Collections;
using UnityEngine;

public class CloudDiskBS : MonoBehaviour
{
    public void SpeedClouds()
    {
        //StartCoroutine(SpC());
        GetComponent<Renderer>().material.SetFloat("_CloudSpeed", 60.0f);
    }

    IEnumerator SpC()
    {
        float f = 30.0f;
        while (f < 60.0f)
        {
            f += 0.1f;
            GetComponent<Renderer>().material.SetFloat("_CloudSpeed", f);
            GetComponent<Renderer>().material.SetFloat("_CloudHeight", f);
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void SlowClouds()
    {
        //StartCoroutine(SlC());
        GetComponent<Renderer>().material.SetFloat("_CloudSpeed", 30.0f);
    }

    IEnumerator SlC()
    {
        float f = 60.0f;
        while (f > 30.0f)
        {
            f -= 0.1f;
            GetComponent<Renderer>().material.SetFloat("_CloudSpeed", f);
            GetComponent<Renderer>().material.SetFloat("_CloudHeight", f);
            yield return new WaitForSeconds(0.01f);
        }
    }
}
