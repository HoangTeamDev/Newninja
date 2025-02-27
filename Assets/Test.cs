using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    public SkeletonDataAsset skeletonDataAsset;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Player1");
        }
    }
}
