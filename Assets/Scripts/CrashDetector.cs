using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] ParticleSystem lossEffect;
    PlayerController playerController;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layerIndex = LayerMask.NameToLayer("Floor");
        if(collision.gameObject.layer == layerIndex )
        {
            playerController.DisableMovement();
            Debug.Log("Loss Game");
            Invoke("ReloadScene", 1f);
            lossEffect.Play();
            

        }
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(1);
    }
}
