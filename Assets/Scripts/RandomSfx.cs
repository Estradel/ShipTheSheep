using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSfx : MonoBehaviour
{
    public AudioClip[] sfxClips;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void PlayRandomSfx()
    {
        if (!_audioSource.isPlaying)
            PlayRandomSfxNow();
    }

    public void PlayRandomSfxNow()
    {
        var randomIndex = Random.Range(0, sfxClips.Length);
        _audioSource.PlayOneShot(sfxClips[randomIndex]);
    }

    public void LogRandomSfx()
    {
        Debug.Log("Random SFX");
    }
}