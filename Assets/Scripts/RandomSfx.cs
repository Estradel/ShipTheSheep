using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class RandomSfx : MonoBehaviour
{

    private AudioSource _audioSource;
    public AudioClip[] sfxClips;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomSfx()
    {
        if (!_audioSource.isPlaying)
            PlayRandomSfxNow();
    }

    public void PlayRandomSfxNow()
    {
        int randomIndex = Random.Range(0, sfxClips.Length);
        _audioSource.PlayOneShot(sfxClips[randomIndex]);
    }

    public void LogRandomSfx()
    {
        Debug.Log("Random SFX");
    }
}
