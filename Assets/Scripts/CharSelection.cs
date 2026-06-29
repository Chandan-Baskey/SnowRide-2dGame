using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelection : MonoBehaviour
{
    [SerializeField] GameObject scoreCanvas;
    [SerializeField] GameObject dino;
    [SerializeField] GameObject frog;
    void Start()
    {
        Time.timeScale = 0;
    }

    void BeginGame()
    {
        Time.timeScale = 1f;
        scoreCanvas.SetActive(true);
        gameObject.SetActive(false);
    }
      
    public void ChooseDino()
    {
        dino.SetActive(true);
        BeginGame();
    }
    public void Choosefrog()
    {
        frog.SetActive(true);
        BeginGame();
    }
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

}
