using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelection : MonoBehaviour
{
    [SerializeField] GameObject scoreCanvas;
    [SerializeField] GameObject Srak;
    [SerializeField] GameObject Chita;
    [SerializeField] GameObject Wolf;
    [SerializeField] AudioClip clickClip;
    AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 0;
        audioSource = GetComponent<AudioSource>();
    }

    void BeginGame()
    {
        Time.timeScale = 1f;
        scoreCanvas.SetActive(true);
        gameObject.SetActive(false);
    }

    void PlayClick()
    {
        if (audioSource != null && clickClip != null)
            audioSource.PlayOneShot(clickClip);
    }

    public void ChooseSrak()
    {
        PlayClick();
        Srak.SetActive(true);
        BeginGame();
    }

    public void ChooseChita()
    {
        PlayClick();
        Chita.SetActive(true);
        BeginGame();
    }

    public void ChooseWolf()
    {
        PlayClick();
        Wolf.SetActive(true);
        BeginGame();
    }

    public void Back()
    {
       
        SceneManager.LoadScene(0);
    }
}