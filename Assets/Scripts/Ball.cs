using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

[DefaultExecutionOrder(0)]
public class Ball : MonoBehaviour
{
    public float ballSpeed;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;
    [NonSerialized] public float ballHalfWidth;
    [NonSerialized] public float ballHalfHeight;
    public Vector2 direction = new Vector2 (0,0);
    public float hitRange;
    public Wall wall;
    public Brick brickManager;

    void Start()
    {
        wall = FindObjectOfType<Wall>();
        brickManager = FindObjectOfType<Brick>();
        ballHalfHeight = transform.localScale.y / 2f;
        ballHalfWidth = transform.localScale.x / 2f;
    }

    void Update()
    {
        Vector2 currentPos = transform.position;
        Vector2 nextPos = currentPos + (ballSpeed * direction * Time.deltaTime);
        Vector2 intersectionPoint = Vector2.zero;
        bool hitSomething = false;

        if (hitSomething == false)
        {
            float closestDistance = float.MaxValue;
            BrickData closestBrick = null;
            Vector2 closestPoint = Vector2.zero;
            Vector2 newDirection = direction;

            foreach (BrickData brick in brickManager.bricks)
            { 
                if (brick.isDestroyed)
                {
                    continue;   
                }
                
                Vector2 bTopLeft = new Vector2(brick.min.x  - ballHalfWidth, brick.max.y + ballHalfHeight);
                Vector2 bTopRight = new Vector2(brick.max.x +ballHalfWidth, brick.max.y + ballHalfHeight);
                Vector2 bBottomLeft = new Vector2(brick.min.x - ballHalfWidth, brick.min.y - ballHalfHeight);
                Vector2 bBottomRight = new Vector2(brick.max.x +ballHalfWidth, brick.min.y -ballHalfHeight);

                bool collided = false;
                Vector2 tempDirection = direction;

                if (direction.y > 0 && LineSegmentIntersection(currentPos, nextPos, bBottomLeft, bBottomRight, out intersectionPoint))
                {
                   tempDirection.y = -tempDirection.y;
                   collided = true;
                }
                else  if (direction.y < 0 && LineSegmentIntersection(currentPos, nextPos, bTopLeft, bTopRight, out intersectionPoint))
                {
                    tempDirection.y = -tempDirection.y;
                    collided = true;
                }
                else if (direction.x > 0 && LineSegmentIntersection(currentPos, nextPos, bBottomLeft, bTopLeft, out intersectionPoint))
                {
                    tempDirection.x = -tempDirection.x;
                    collided = true;
                }
                else if (direction.x < 0 && LineSegmentIntersection(currentPos, nextPos, bBottomRight, bTopRight, out intersectionPoint))
                {
                    tempDirection.x = -tempDirection.x;
                    collided = true;
                }

                if (collided)
                {
                    float dist = Vector2.Distance(currentPos, intersectionPoint);

                    if (dist < closestDistance)
                    {
                        closestDistance = dist;
                        closestBrick = brick;
                        closestPoint = intersectionPoint;
                        newDirection = tempDirection;
                    }
                }
            }

            if (closestBrick != null)
            {
                hitSomething = true;
                direction = newDirection;
                closestBrick.isDestroyed = true;
                Destroy(closestBrick.image);

             
                nextPos = closestPoint + direction * 0.01f;
            }
        }

        if (hitSomething == false)
        {
            Vector2 wallPos = wall.transform.position;
            float halfW = wall.wallWidth / 2f;
            float halfH = wall.wallHeight / 2f;

            Vector2 wMin = new Vector2(wallPos.x - halfW, wallPos.y - halfH);
            Vector2 wMax = new Vector2(wallPos.x + halfW, wallPos.y + halfH);

            Vector2 wTopLeft = new Vector2(wMin.x - ballHalfWidth, wMax.y + ballHalfHeight);
            Vector2 wTopRight = new Vector2(wMax.x + ballHalfWidth, wMax.y + ballHalfHeight);
            Vector2 wBottomLeft = new Vector2(wMin.x - ballHalfWidth, wMin.y - ballHalfHeight);
            Vector2 wBottomRight = new Vector2(wMax.x + ballHalfWidth, wMin.y - ballHalfHeight);

            if (direction.y < 0 && LineSegmentIntersection(currentPos, nextPos, wTopLeft, wTopRight, out intersectionPoint))
            {
                float hit = intersectionPoint.x - wallPos.x;
                float hitValue = (hit / halfW) * hitRange;
                hitValue = Mathf.Clamp(hitValue, -hitRange, hitRange);

                float rad = hitValue * Mathf.Deg2Rad;

                direction = new Vector2(Mathf.Sin(rad), Mathf.Abs(Mathf.Cos(rad))).normalized;
                hitSomething = true;
            }
            else if (direction.x > 0 && LineSegmentIntersection(currentPos, nextPos, wBottomLeft, wTopLeft, out intersectionPoint))
            {
                direction.x = -direction.x;
                if (transform.position.y > wallPos.y)
                {
                    direction.y = Mathf.Abs(direction.y);
                }
                hitSomething = true;
            }
            else if (direction.x < 0 && LineSegmentIntersection(currentPos, nextPos, wBottomRight, wTopRight, out intersectionPoint))
            {
                direction.x = -direction.x;
                if (transform.position.y > wallPos.y)
                {
                    direction.y = Mathf.Abs(direction.y);
                }
                hitSomething = true;
            }

            if (hitSomething)
            {
                nextPos = intersectionPoint + (direction * 0.01f);
            }
        }

        if (nextPos.x + ballHalfWidth > maxX && direction.x > 0)
            { 
                float limitX = maxX - ballHalfWidth;
                float overshoot = nextPos.x - limitX;

                direction.x = -direction.x;
                nextPos.x = limitX - overshoot;
            }
        
            else if (nextPos.x - ballHalfWidth < minX && direction.x < 0)
            {
                float limitX = minX + ballHalfWidth;
                float overshoot = limitX - nextPos.x;

                direction.x = -direction.x;
                nextPos.x = limitX + overshoot;
            }

           
            if (nextPos.y + ballHalfHeight > maxY && direction.y > 0)
            { 
                float limitY = maxY - ballHalfHeight;
                float overshoot = nextPos.y - limitY;

                direction.y = -direction.y; 
                nextPos.y = limitY - overshoot;
            }
       
            else if (nextPos.y - ballHalfHeight < minY)
            {
            float randomX = Random.Range(minX + 0.5f, maxX - 0.5f);
            transform.position = new Vector3(randomX, 0f, 0f);
            direction = new Vector2(Random.Range(-1f, 1f), -1f).normalized;
            return; 
            }
            transform.position = nextPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start =transform.position;
        Vector3 end = start + (Vector3)(direction * 3f);
        Gizmos.DrawLine(start, end);
    }

 
    bool LineSegmentIntersection(Vector2 start, Vector2 end, Vector2 start2, Vector2 end2, out Vector2 intersectionPoint)
    {
        Vector2 line1 = end - start; 
        Vector2 line2 = end2 - start2;
        float determinant = line1.x * line2.y - line1.y * line2.x;

        if (determinant == 0)
        {
            intersectionPoint = Vector2.zero;
            return false;
        }
        float rat2 = ((start2.x - start.x) * line1.y - (start2.y - start.y) * line1.x) / determinant;
        float rat1 = ((start2.x - start.x) * line2.y - (start2.y - start.y) * line2.x) / determinant;

        if (-0.01f <= rat2 && rat2 <= 1.01f && -0.01f <= rat1 && rat1 <= 1.01f)
        {
            intersectionPoint = start + (rat1 * line1);
            return true;
        }
        intersectionPoint = Vector2.zero;
        return false;
    }
}
