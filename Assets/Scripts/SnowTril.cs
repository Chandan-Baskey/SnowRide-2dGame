using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SnowTril : MonoBehaviour
{
    [SerializeField] ParticleSystem snowEffect;
    AudioSource audioSource;
    [SerializeField] AudioClip snowSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = snowSound;
        audioSource.loop = true; // keep looping while touching floor
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            snowEffect.Play();

            if (!audioSource.isPlaying)
                audioSource.Play();


        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            snowEffect.Stop();
            audioSource.Stop();
        }
    }
    
}
