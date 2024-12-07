using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour
{
    public AudioClip otherClip;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = otherClip;
            audioSource.Play();
        }
    }
}