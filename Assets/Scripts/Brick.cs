using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Ball currentBall;
    public float ballRadius;
    public float ballXwidth;
    public List<BrickData> bricks = new List<BrickData>();

    void Start()
    {
        foreach (Transform categoryFolder in transform)
        {
            string folderName = categoryFolder.name.ToLower();

            foreach (Transform brickTransform in categoryFolder)
            {
                GameObject brickObj = brickTransform.gameObject;
                Renderer rend = brickObj.GetComponentInChildren<Renderer>();
                if (rend != null)
                {
                    Vector2 pos = rend.bounds.center;
                    Vector2 size = rend.bounds.size;
                    bricks.Add(new BrickData(pos, size, brickObj, folderName));
                }
            }
        }
    }

    void Update()
    {
        foreach (BrickData brick in bricks)
        {
            if (brick.isDestroyed) continue;

          
            if (brick.waveDelay > 0)
            {
                brick.waveDelay -= Time.deltaTime;
                continue;
            }

            if (brick.shakeTimer > 0)
            {
                brick.shakeTimer -= Time.deltaTime; 
                float maxDuration = 0.25f;

                float elapsedTime = maxDuration - brick.shakeTimer; 
   
                float frequency = 50f; 
                float dampingFactor = brick.shakeTimer / maxDuration; 
                float springOffset = Mathf.Cos(elapsedTime * frequency) * dampingFactor * brick.currentMagnitude;

                brick.image.transform.position = (Vector3)brick.originalPosition + (Vector3)(brick.shakeDirection * springOffset);
            }
            else
            {
              
                brick.image.transform.position = brick.originalPosition;
            }
        }
    
    }
}

public class BrickData
{
    public Vector2 min;
    public Vector2 max;
    public bool isDestroyed = false;
    public GameObject image;
    public string colorName;

    public Vector2 originalPosition;
    public float shakeTimer;
    public float waveDelay;
    public float currentMagnitude;
    public Vector2 shakeDirection;

    public BrickData(Vector2 pos, Vector2 size, GameObject g, string color)
    {
        min = new Vector2(pos.x - size.x / 2f, pos.y - size.y / 2f);
        max = new Vector2(pos.x + size.x / 2f, pos.y + size.y / 2f);
        image = g;
        colorName = color;

        originalPosition = g.transform.position;
        shakeTimer = 0f;
        waveDelay = 0f;
        currentMagnitude = 0f;
        shakeDirection = Vector2.zero;
    }
}