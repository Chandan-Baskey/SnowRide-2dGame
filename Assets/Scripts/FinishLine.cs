using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] ParticleSystem WinEffect;
    [SerializeField] AudioClip gameWinClip;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameWinClip ;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        

        if(collision.gameObject.layer == layerIndex )
        {
            Debug.Log("Win");
            Invoke("ReloadScene", 1f);
            WinEffect.Play();
            
            if (audioSource != null && gameWinClip != null)
            {
                audioSource.PlayOneShot(gameWinClip);
            }
        }
       
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
