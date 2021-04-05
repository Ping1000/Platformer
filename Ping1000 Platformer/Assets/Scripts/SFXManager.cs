using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum volumeType {
    veryQuiet,
    quiet,
    half,
    loud,
    veryLoud
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    public static float veryQuietVolume = 0.1f;
    public static float quietVolume = 0.2f;
    public static float halfVolume = 0.4f;
    public static float loudVolume = 0.6f;
    public static float veryLoudVolume = 0.8f;

    public AudioSource playerFootstepSrc;
    public AudioSource miscSource1;
    public AudioSource miscSource2;

    private bool playerExists = false;
    private EricCharacterMovement player;
    private string[] playerFootstepClips;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        player = FindObjectOfType<EricCharacterMovement>();
        if (player != null && playerFootstepSrc != null) {
            playerExists = true;
            playerFootstepClips = new string[9];
            for (int i = 0; i < 9; i++) {
                playerFootstepClips[i] = "Footstep_" + (i + 1).ToString();
            }
            InvokeRepeating("PlayPlayerFootsteps", 0, 0.75f);
        }
    }

    public void PlayPlayerFootsteps() {
        string clipName = playerFootstepClips[Random.Range(0, playerFootstepClips.Length)];
        playerFootstepSrc.clip = Resources.Load("Audio/SFX/Footsteps/" + clipName) as AudioClip;
        playerFootstepSrc.volume = (!player.grounded) ? 0 : 
            (Mathf.Abs(player._rb.velocity.x) / player.maxSpeed);
        playerFootstepSrc.Play();
    }

    /// <summary>
    /// Plays a specific song at the specified volume level.
    /// </summary>
    /// <param name="path">The path to the audio clip, starting in Resources</param>
    /// <param name="volumeMode">The volume mode/level to play at</param>
    public static void PlayNewSound(string path, volumeType volumeMode, float pitch = 1f) {
        if (path == null || path == "")
            return;

        GameObject sfxPlayer = Instantiate(Resources.Load("Audio/SFX Player") as GameObject);
        AudioSource playerSrc = sfxPlayer.GetComponent<AudioSource>();
        playerSrc.clip = Resources.Load(path) as AudioClip;

        switch (volumeMode) {
            case volumeType.veryQuiet:
                playerSrc.volume = veryQuietVolume;
                break;
            case volumeType.quiet:
                playerSrc.volume = quietVolume;
                break;
            case volumeType.half:
                playerSrc.volume = halfVolume;
                break;
            case volumeType.loud:
                playerSrc.volume = loudVolume;
                break;
            case volumeType.veryLoud:
                playerSrc.volume = veryLoudVolume;
                break;
        }

        playerSrc.pitch = pitch;
        playerSrc.Play();
        Destroy(playerSrc.gameObject, playerSrc.clip.length);
    }

    public void StartMissileSound() {
        miscSource1.clip = Resources.Load("Audio/SFX/Missile_Launch") as AudioClip;
        miscSource1.volume = quietVolume;
        miscSource1.Play();
        if (!miscSource2.isPlaying) {
            miscSource2.clip = Resources.Load("Audio/SFX/Missile_Travel") as AudioClip;
            StartCoroutine(FadeIn(miscSource2, miscSource1.clip.length, quietVolume));
        }
    }

    public void MissileImpactSound() {
        miscSource1.clip = Resources.Load("Audio/SFX/Missile_Impact") as AudioClip;
        miscSource1.Play();
        if (FindObjectsOfType<MissileController>().Length <= 1) {
            Invoke("StopMissileTravel", 0.2f);
        }
    }

    public void StopMissileTravel() {
        miscSource2.Stop();
    }

    IEnumerator FadeIn(AudioSource src, float fadeTime = 2f, float maxVolume = 1f) {
        if (!src.isPlaying)
            src.Play();
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime) {
            src.volume = Mathf.Lerp(0, maxVolume, progress / fadeTime);
            yield return null;
        }
    }

    IEnumerator FadeOut(AudioSource src, float fadeTime = 2f, float maxVolume = 1f) {
        for (float progress = 0; progress < fadeTime; progress += Time.deltaTime) {
            src.volume = Mathf.Lerp(maxVolume, 0, progress / fadeTime);
            yield return null;
        }
    }

    public void CheckMissileSounds() {
        if (FindObjectsOfType<MissileController>().Length == 0) {
            miscSource2.Stop();
        } else {
            if (!miscSource2.isPlaying) {
                miscSource2.Play();
            }
        }
    }

    //public void NukeChargeSound() {
    //    miscSource1.clip = Resources.Load("Audio/SFX/Pulse_Charge") as AudioClip;

    //}
}