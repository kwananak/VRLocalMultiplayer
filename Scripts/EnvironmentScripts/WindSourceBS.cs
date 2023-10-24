using System.Collections;
using UnityEngine;

public class WindSourceBS : MonoBehaviour
{
    float targetStrength;
    float strength;
    [SerializeField] AudioSource lowWind;
    [SerializeField] AudioSource highWind;
    [SerializeField] float windVolume = 0.5f;

    private void Start()
    {
        StartCoroutine(FadeWind("low", "in"));
    }

    void Update()
    {
        /*targetStrength += (Random.value - 0.5f) * 10f;
        if (targetStrength > strength + 1f) strength += 0.5f; else if (targetStrength > strength - 1f) strength -= 0.5f; else strength = targetStrength;
        GetComponent<AudioLowPassFilter>().cutoffFrequency = 1000f + strength;*/
    }

    public void StartStrongWind()
    {
        StartCoroutine(FadeWind("low", "out"));
        highWind.volume = 1.0f;
        highWind.Play();
    }

    public void StopStrongWind()
    {
        StartCoroutine(FadeWind("high", "out")); 
        StartCoroutine(FadeWind("low", "in"));
    }

    IEnumerator FadeWind(string wind, string way)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            if (wind == "low")
            {
                if (way == "in")
                {
                    if (lowWind.volume < windVolume)
                    {
                        lowWind.volume += 0.1f;
                    }
                    else
                    {
                        yield break;
                    }
                }
                else
                {
                    if (lowWind.volume > 0)
                    {
                        lowWind.volume -= 0.1f;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
            else
            {
                if (way == "in")
                {
                    if (highWind.volume < windVolume)
                    {
                        highWind.volume += 0.1f;
                    }
                    else
                    {
                        yield break;
                    }
                }
                else
                {
                    if (highWind.volume > 0)
                    {
                        highWind.volume -= 0.1f;
                    }
                    else
                    {
                        yield break;
                    }
                }
            }
        }
        
    }
}
