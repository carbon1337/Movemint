using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public GameObject[] extraSFXs;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        PlayRandomMusic();
    }

    void Update() {
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");

        extraSFXs = GameObject.FindGameObjectsWithTag("SFX Source");

        foreach (GameObject extraSFX in extraSFXs) {
            extraSFX.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFXVolume");
        }
    }

    public void PlayRandomMusic() {

        int randomIndex = UnityEngine.Random.Range(0, musicSounds.Length);
        musicSource.clip = musicSounds[randomIndex];
        musicSource.Play();
        StartCoroutine(WaitForSongEnd());


    }

    IEnumerator WaitForSongEnd()
    {
        yield return new WaitForSeconds(musicSource.clip.length);
        PlayRandomMusic();
    }

    public void PlaySFX(string name, bool varyPitch, float pitch) {
        AudioClip s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null) {
            Debug.Log("Sound not found!");
        } else {
            sfxSource.clip = s;

            if(varyPitch) {
                sfxSource.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            } else {
                sfxSource.pitch = pitch;
            }

            sfxSource.Play();
        }

    }
}
