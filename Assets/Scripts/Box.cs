using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public CharacterAuto _CharacterAuto;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Box2"))
        {
            if (collision.gameObject == _CharacterAuto._targetMove)
            {
                _CharacterAuto._isBox = true;

            }
        }
    }

}
