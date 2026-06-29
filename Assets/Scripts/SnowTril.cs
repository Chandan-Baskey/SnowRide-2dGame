using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class SnowTril : MonoBehaviour
{
    [SerializeField] ParticleSystem snowEffect;
    //[SerializeField] AudioSource audioSource;

    private void Start()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            snowEffect.Play();
            //audioSource.Play();
            
            
            
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if (collision.gameObject.layer == layerIndex)
        {
            snowEffect.Stop();
            //audioSource.Stop();   
        }
    }
    void Clip()
    {

    }
}
