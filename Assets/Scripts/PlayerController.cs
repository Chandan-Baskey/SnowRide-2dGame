using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [SerializeField] float torqueAmount;
    [SerializeField] float baseSpeed = 15f;
    [SerializeField] float boostSpeed = 20f;
    [SerializeField] ScoreManager addScore;
    Rigidbody2D player;
    SurfaceEffector2D surfaceEffector;
    bool canplayerMove = true;

    float previousRotation;
    float totalRotation;
    float flipCount;

    
    

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        surfaceEffector = FindFirstObjectByType<SurfaceEffector2D>();
    }

    void Update()
    {
        if(canplayerMove)
        {
            RotatePlayer();
            BoostPlayer();
            CalculateFlips();
        }
       
    }
    void RotatePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");

        if (moveX < 0)
        {
            player.AddTorque(torqueAmount * Time.deltaTime);
        }
        else if (moveX > 0)
        {
            player.AddTorque(-torqueAmount * Time.deltaTime);
        }
    }

    void BoostPlayer()
    {
        float moveY = Input.GetAxis("Vertical");
        if (moveY > 0)
        {
            surfaceEffector.speed = boostSpeed;
        }
        else
        {
            surfaceEffector.speed = baseSpeed;
        }
    }

    void CalculateFlips()
    {
        float currentRotation = transform.rotation.eulerAngles.z;
        totalRotation += Mathf.DeltaAngle(previousRotation, currentRotation); 
        if(totalRotation>340 || totalRotation<-340)
        {
            flipCount += 1;
            totalRotation = 0;
            print(flipCount);
            addScore.AddScore(1);  
        }
        
            
        
        previousRotation = currentRotation;
    }
    public void DisableMovement()
    {
        canplayerMove = false;
    }
    
}
