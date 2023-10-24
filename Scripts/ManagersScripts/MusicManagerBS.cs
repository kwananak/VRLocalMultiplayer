using System.Collections;
using UnityEngine;

public class MusicManagerBS : MonoBehaviour
{
    public static MusicManagerBS Instance { get; private set; }
    AudioSource music;
    [SerializeField] float maxVolume;
    [SerializeField] float fadeStep;
    [SerializeField] float waitForRestart;

    private void Start()
    {
        if (Instance != null && Instance != this) Destroy(this); else Instance = this;
        music = GetComponent<AudioSource>();
        StartCoroutine(StartLow());
    }

    public void StartHype()
    {
        music.Stop();
        music.clip = Resources.Load<AudioClip>("HypeTrack");
        music.volume = maxVolume;
        music.Play();
    }

    IEnumerator StartLow()
    {
        music.Stop();
        music.clip = Resources.Load<AudioClip>("LowTrack");
        music.Play();
        while (music.volume < maxVolume)
        {
            music.volume += fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeOutLow()
    {
        while (music.volume > 0.0f)
        {
            music.volume -= fadeStep * 3;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator FadeOutHype()
    {
        while (music.volume > 0.0f)
        {
            if (GameManagerBS.Instance.gameStarted) break;
            music.volume -= fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(waitForRestart);
        if (GameManagerBS.Instance == null || !GameManagerBS.Instance.gameStarted) StartCoroutine(StartLow());
    }

    public IEnumerator FadeOutHype(int waitTime)
    {
        while (music.volume > 0.0f)
        {
            if (GameManagerBS.Instance.gameStarted) break;
            music.volume -= fadeStep;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(waitTime);
        if (GameManagerBS.Instance == null || !GameManagerBS.Instance.gameStarted) StartCoroutine(StartLow());
    }
}
