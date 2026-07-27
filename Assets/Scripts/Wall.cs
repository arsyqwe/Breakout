using System;
using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(1)]
public class Wall : MonoBehaviour
{
    public float maxX;
    public float minX;
    public float wallSpeed;
    public float wallWidth;
    public float wallHeight;
    public float screenRightEdge;
    public float screenLeftEdge;
    [NonSerialized] public Renderer rend;


    public float maxTiltAngle = 25f;   
    public float springFrequency = 20f;  
    public float springDuration = 0.45f;

    private float currentTiltAngle = 0f;
    private float tiltTimer = 0f;
   

    void Start()
    {
        rend = GetComponentInChildren<Renderer>();
        // float halfScreenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        // maxX = halfScreenWidth - (wallWidth / 2f);    
        // minX = -halfScreenWidth + (wallWidth / 2f);
        wallHeight = rend.bounds.size.y;
        wallWidth = rend.bounds.size.x;
    }

    void Update()
    {
        Vector2 wallPos = transform.position;
        float step = wallSpeed * Time.deltaTime;
        if (Keyboard.current.dKey.isPressed)
        {
            if (wallPos.x + step > maxX)
            {
                wallPos.x = maxX;
            }
            else
            {
                wallPos.x += step;
            }
        }

        if (Keyboard.current.aKey.isPressed)
        {
            if (wallPos.x - step < minX)
            {
                wallPos.x = minX;
            }
            else
            {
                wallPos.x -= step;
            }
        }
        transform.position = wallPos;

       
        if (tiltTimer > 0)
        {
            tiltTimer -= Time.deltaTime;

            float elapsedTime = springDuration - tiltTimer;
            float t = elapsedTime / springDuration;

            float springOffset = Mathf.Sin(t * springFrequency) * (1f - t) * currentTiltAngle;

            transform.rotation = Quaternion.Euler(0, 0, springOffset);
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }

        /* if (Keyboard.current.digit1Key.isPressed)
         {
             Application.targetFrameRate = 3;
         }
         if (Keyboard.current.digit2Key.isPressed)
         {
             //Application.targetFrameRate = 15;
         }
         if (Keyboard.current.digit3Key.isPressed)
         {
             Application.targetFrameRate = 30;
         }
         if (Keyboard.current.digit4Key.isPressed)
         {
             Application.targetFrameRate = 60;
         }*/
    }

  
    public void TriggerSpringTilt(float hitPositionX)
    { 
        currentTiltAngle = -hitPositionX * maxTiltAngle;
        tiltTimer = springDuration;
    }
}