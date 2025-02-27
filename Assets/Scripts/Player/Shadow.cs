using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public Transform character;  
    public LayerMask groundLayer;

    protected  void LateUpdate()
    {      
        RaycastHit2D hit = Physics2D.Raycast(character.position, Vector2.down, Mathf.Infinity, groundLayer);

        if (hit.collider != null)
        {          
            transform.position = new Vector3(character.position.x, hit.point.y, character.position.z);
        }
    }
}
