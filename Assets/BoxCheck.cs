using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxCheck : MonoBehaviour
{
    public CharacterAuto _characterAuto;
    public CharacterMove _characterMove;
    public List<Transform> boxes; // Danh sách tất cả các box
    public Transform player;       // Vị trí của người chơi
    public Transform destination;  // Vị trí box đích
    public float maxYDistance = 3f; // Khoảng cách tối đa theo trục Y
    public float maxEdgeDistance = 2f; // Khoảng cách từ mép box đến mép box tiếp theo
    List<Transform> path=null;
    void Start()
    {
        
        path = FindPath(player, destination);

        // In ra danh sách các box cần đi qua
        if (path != null)
        {
            foreach (Transform box in path)
            {
                Debug.Log("Box cần đi qua: " + box.name);
            }
        }
        else
        {
            Debug.Log("Không tìm thấy đường đi.");
        }
    }

    List<Transform> FindPath(Transform startBox, Transform endBox)
    {
        Queue<Transform> queue = new Queue<Transform>();
        Dictionary<Transform, Transform> cameFrom = new Dictionary<Transform, Transform>(); 
        HashSet<Transform> visited = new HashSet<Transform>(); 
        queue.Enqueue(startBox);
        visited.Add(startBox);

        while (queue.Count > 0)
        {
            Transform currentBox = queue.Dequeue();

            if (currentBox == endBox)
            {
                return RetracePath(startBox, endBox, cameFrom);
            }

           
            List<Transform> neighbors = GetNeighbors(currentBox);
            foreach (Transform neighbor in neighbors)
            {
                if (!visited.Contains(neighbor) && IsValidMove(currentBox, neighbor))
                {
                    visited.Add(neighbor);
                    queue.Enqueue(neighbor);
                    cameFrom[neighbor] = currentBox;
                }
            }
        }

        return null; 
    }

    List<Transform> GetNeighbors(Transform currentBox)
    {
        List<Transform> neighbors = new List<Transform>();

        foreach (Transform box in boxes)
        {
          
            if (box != currentBox)
            {
                BoxCollider2D boxCollider2D =box.gameObject.GetComponent<BoxCollider2D>();
                BoxCollider2D boxCollider2D1 =currentBox.gameObject.GetComponent<BoxCollider2D>();
                if(Mathf.Abs(boxCollider2D.bounds.max.y-boxCollider2D1.bounds.max.y)<=3)
                {
                    if (destination.position.y > currentBox.position.y)
                    {
                        if (boxCollider2D.bounds.max.y > boxCollider2D1.bounds.max.y)
                        {
                            neighbors.Add(box);
                        }
                    }
                    else
                    {
                        if (boxCollider2D.bounds.max.y < boxCollider2D1.bounds.max.y)
                        {
                            neighbors.Add(box);
                        }
                    }
                  
                }
                
            }
        }

        return neighbors;
    }

    bool IsValidMove(Transform fromBox, Transform toBox)
    {
        
        
        BoxCollider2D boxCollider = fromBox.gameObject.GetComponent<BoxCollider2D>();
        BoxCollider2D boxCollider1=toBox.gameObject.GetComponent<BoxCollider2D>();
        float xMaxcurrent=boxCollider.bounds.max.x;     
        float xMinCurrent=boxCollider.bounds.min.x;   
        float xMaxTo=boxCollider1.bounds.max.x;  
        float xMinTo=boxCollider1.bounds.min.x;
        if (xMinTo > xMaxcurrent)
        {
            float distanceX = Mathf.Abs(xMinTo - xMaxcurrent);
            if (distanceX <= 2)
            {
                return true;
            }
        }
        if (xMaxTo < xMinCurrent)
        {
            float distanceX = Mathf.Abs(xMaxTo - xMinCurrent);
            if (distanceX <= 2)
            {
                return true;
            }
        }
        if ((xMaxTo >= xMaxcurrent) && (xMinTo <= xMinCurrent))
        {
            return true;
        }
        if(xMinCurrent>xMinTo&&xMinCurrent<xMaxTo&&xMaxcurrent>xMaxTo||xMaxcurrent>xMaxTo&&xMaxcurrent>xMinTo&& xMinCurrent < xMinTo)
        {
            return true ;
        }


        return false;
    }

    List<Transform> RetracePath(Transform startBox, Transform endBox, Dictionary<Transform, Transform> cameFrom)
    {
        List<Transform> path = new List<Transform>();
        Transform currentBox = endBox;

        while (currentBox != startBox)
        {
            path.Add(currentBox);
            currentBox = cameFrom[currentBox];
        }
        path.Reverse();
        return path;
    }


}
