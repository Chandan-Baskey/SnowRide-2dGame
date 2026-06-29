using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    [SerializeField] ParticleSystem WinEffect;
    
    
    private void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Player");
        

        if(collision.gameObject.layer == layerIndex )
        {
            Debug.Log("Win");
            Invoke("ReloadScene", 1f);
            WinEffect.Play();
            
        }
       
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
