using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour {
    [SerializeField] private LayerMask m_GroundLayer;
    [SerializeField] private Vector2 m_Dirrection;
    [SerializeField] private float m_Length;

    public bool Ignore = true;

    //public void Update() {
    //    print(IsGrounded());
    //}

    public bool IsGrounded(int direction) {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_Dirrection.normalized * new Vector2(direction, 1), m_Length, m_GroundLayer);
        Debug.DrawRay(transform.position, m_Dirrection.normalized * new Vector2(direction, 1) * m_Length, Color.red);
        return hit.collider != null;
    }
}
