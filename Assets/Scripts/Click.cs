using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    protected  void OnEnable()
    {
        Invoke("Hide", 0.5f);
    }
    void Hide()
    {
        gameObject.SetActive(false);
        PoolingContronller.Instance.ReturnClick(gameObject);
    }
}
