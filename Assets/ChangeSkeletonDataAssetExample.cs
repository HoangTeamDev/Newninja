using Spine.Unity;
using UnityEngine;

public class ChangeSkeletonDataAssetExample : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; 
    public SkeletonDataAsset newSkeletonDataAsset; 

    void Update()
    {
        // Nhấn phím Space để thay đổi SkeletonDataAsset
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSkeletonData(newSkeletonDataAsset);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
        }
    }

    void ChangeSkeletonData(SkeletonDataAsset skeletonDataAsset)
    {
        if (skeletonDataAsset == null)
        {
            Debug.LogWarning("SkeletonDataAsset is null!");
            return;
        }

       
        skeletonAnimation.skeletonDataAsset = skeletonDataAsset;

       
        skeletonAnimation.Initialize(true);

        Debug.Log($"SkeletonDataAsset changed to: {skeletonDataAsset.name}");
    }
}
