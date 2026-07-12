using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Ball currentBall;
    public float ballRadius;
    public float ballXwidth;
    public List<BrickData> bricks = new List<BrickData>();
    public GameObject brickPrefab;
    void Start()
    { 
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector2 pos = new Vector2(-7.2f + (i * 1.6f), 10f + (j * 1f));
                Vector2 size = new Vector2(1.4f, 0.8f);

                GameObject image = Instantiate(brickPrefab, pos, Quaternion.identity);
                image.transform.localScale = size;

                bricks.Add(new BrickData(pos, size, image ));
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

    public BrickData(Vector2 pos, Vector2 size, GameObject g)
    {
        min = new Vector2(pos.x - size.x / 2f  , pos.y - size.y / 2f);
        max = new Vector2(pos.x + size.x / 2f , pos.y + size.y / 2f );
        image = g;
    }
}