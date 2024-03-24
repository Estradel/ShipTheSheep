using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public AudioClip footstepSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Check if velocity is greater than 1
        if (rb.velocity.magnitude > 1f && !audioSource.isPlaying)
        {
            // Play footstep sound
            audioSource.PlayOneShot(footstepSound, 0.2f);
        }
    }
}
