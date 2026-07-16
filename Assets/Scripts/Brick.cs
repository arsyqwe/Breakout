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
}
public class BrickData
{
    public Vector2 min;
    public Vector2 max;
    public bool isDestroyed = false;
    public GameObject image;
    public string colorName; 

    public BrickData(Vector2 pos, Vector2 size, GameObject g, string color)
    {
        min = new Vector2(pos.x - size.x / 2f, pos.y - size.y / 2f);
        max = new Vector2(pos.x + size.x / 2f, pos.y + size.y / 2f);
        image = g;
        colorName = color; 
    }
}