using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    int score = 0;
    

    // Update is called once per frame
    void Update()
    {
       
    }
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Flip: " + score;
    }
}
