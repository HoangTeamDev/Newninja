using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAutoTest : MonoBehaviour
{
    public Vector2 targetPosition;   // Vị trí B
    public float moveSpeed = 5f;      // Tốc độ di chuyển
    public LayerMask obstacleLayer;   // Lớp vật cản

    void Update()
    {
        MoveTowardsTarget();
    }

    void MoveTowardsTarget()
    {
        Vector2 currentPosition = transform.position;      
        float distanceX = targetPosition.x - currentPosition.x;
        float distanceY = targetPosition.y - currentPosition.y;       
        bool canMoveX = !Physics2D.Raycast(currentPosition, Vector2.right * Mathf.Sign(distanceX), Mathf.Abs(distanceX), obstacleLayer);
        bool canMoveY = !Physics2D.Raycast(currentPosition, Vector2.up * Mathf.Sign(distanceY), Mathf.Abs(distanceY), obstacleLayer);      
        if (canMoveX)
        {            
            float stepX = Mathf.Sign(distanceX) * moveSpeed * Time.deltaTime;
            transform.Translate(stepX, 0, 0);
        }
        else if (canMoveY)
        {
          
            float stepY = Mathf.Sign(distanceY) * moveSpeed * Time.deltaTime;
            transform.Translate(0, stepY, 0);
        }
        else
        {
           
            float stepX = Mathf.Sign(distanceX) * moveSpeed * Time.deltaTime;
            float stepY = Mathf.Sign(distanceY) * moveSpeed * Time.deltaTime;
            transform.Translate(stepX, stepY, 0);
        }
    }
}

