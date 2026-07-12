
using System.Collections.Generic;
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

    void Start()
    {
        float halfScreenWidth = Camera.main.aspect * Camera.main.orthographicSize;
        maxX = halfScreenWidth - (wallWidth / 2f);
        minX = -halfScreenWidth + (wallWidth / 2f);

        wallWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        wallHeight = GetComponent<SpriteRenderer>().bounds.size.y;
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


       /* if (Keyboard.current.digit1Key.isPressed)
        {
            Application.targetFrameRate = 3;
        }
        if (Keyboard.current.digit2Key.isPressed)
        {
            Application.targetFrameRate = 15;
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

}