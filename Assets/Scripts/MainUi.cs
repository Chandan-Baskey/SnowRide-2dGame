using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUi : MonoBehaviour
{
    [SerializeField] AudioClip clickClip;
    AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    public void Play()
    {
        StartCoroutine(PlayThenLoad());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator PlayThenLoad()
    {
        audioSource.PlayOneShot(clickClip);
        yield return new WaitForSeconds(clickClip.length);
        SceneManager.LoadScene(1);
    }
}
