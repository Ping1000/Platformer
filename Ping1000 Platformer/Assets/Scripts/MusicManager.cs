using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource musicSrc;
    public float fadeTime;

    public static MusicManager instance;
    public static float maxVolume = 0.2f;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSrc.Play();
        musicSrc.loop = true;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn() {
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime) {
            musicSrc.volume = Mathf.Lerp(0, maxVolume, progress / fadeTime);
            yield return null;
        }
    }
}
