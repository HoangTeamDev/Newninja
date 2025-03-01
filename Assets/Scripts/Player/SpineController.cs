using UnityEngine;
using Spine.Unity;
[System.Serializable]
public class SpineController : MonoBehaviour
{
    [Header("Spine Components")]
    public SkeletonAnimation _skeletonAnimation;
    public string _currentStatus;
    [Header("Spine Configuration")]
    public SkeletonDataAsset _defaultSkeletonDataAsset;
    public bool _isSkill;
    public Rigidbody2D _rb;
     void Start()
    {


    }

    void OnDestroy()
    {
        if (_skeletonAnimation != null && _skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.Event -= HandleSpineEvent;
        }
    }
    public  string GetStatus(int id)
    {
        switch (id)
        {
            case 0:
                return PlayerAnimationState.PlayerIdle.ToString();
            case -1:
                return PlayerAnimationState.PlayerRun.ToString();
            case -2:
                return PlayerAnimationState.PlayerJump.ToString();
            case -3:
                return PlayerAnimationState.PlayerDoubleJump.ToString();
            case -4:
                return PlayerAnimationState.PlayerFall.ToString();
            case -5:
                return PlayerAnimationState.PlayerDie.ToString();
            case -6:
                return PlayerAnimationState.PlayerBlock.ToString();
            case 1:
                return PlayerAnimationState.PlayerSkill1.ToString();
            case 2:
                return PlayerAnimationState.PlayerSkill2.ToString();
            case 3:
                return PlayerAnimationState.PlayerSkill3.ToString();
            case 4:
                return PlayerAnimationState.PlayerSkill4.ToString();
            case 5:
                return PlayerAnimationState.PlayerSkill5.ToString();
            case 6:
                return PlayerAnimationState.PlayerSkill6.ToString();
            case 7:
                return PlayerAnimationState.PlayerSkill7.ToString();
            case 8:
                return PlayerAnimationState.PlayerSkill8.ToString();
            case 9:
                return PlayerAnimationState.PlayerSkill9.ToString();
            case 10:
                return PlayerAnimationState.PlayerSkill10.ToString();
            case 11:
                return PlayerAnimationState.PlayerSkill11.ToString();
            case 12:
                return PlayerAnimationState.PlayerSkill12.ToString();

        }
        return null;
    }
    private void HandleSpineStart(Spine.TrackEntry trackEntry)
    {

    }
    private void HandleSpineComplete(Spine.TrackEntry trackEntry)
    {

        if (trackEntry.Animation.Name == GetStatus(1))
        {

            _isSkill = false;
            _rb.gravityScale = 3;
        }
    }
    private void HandleSpineEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "EventActive")
            switch (e.Int)
            {
                case 0:
                    break;

            }
    }

    public void PlayAnimation(int id, bool loop = false, float timescale = 1f, int t = 0)
    {
        string animationName = GetStatus(id);
        if (_currentStatus != animationName)
        {
            _currentStatus = animationName;
            if (_skeletonAnimation.skeleton.Data.FindAnimation(animationName) != null)
            {
                _skeletonAnimation.AnimationState.SetAnimation(t, animationName, loop);
                _skeletonAnimation.timeScale = timescale;
            }
        }

    }


    public void ChangeSkin(string skinName)
    {
        if (_skeletonAnimation.skeleton.Data.FindSkin(skinName) != null)
        {
            _skeletonAnimation.skeleton.SetSkin(skinName);
            _skeletonAnimation.skeleton.SetSlotsToSetupPose();
        }
    }

    public void ChangeSkeletonData(SkeletonDataAsset skeletonDataAsset)
    {
        if (skeletonDataAsset == null)
        {
            return;
        }
        _skeletonAnimation.AnimationState.Event -= HandleSpineEvent;
        _skeletonAnimation.AnimationState.Complete -= HandleSpineComplete;
        _skeletonAnimation.AnimationState.Start -= HandleSpineStart;
        _skeletonAnimation.skeletonDataAsset = skeletonDataAsset;
        _skeletonAnimation.Initialize(true);
        _skeletonAnimation.AnimationState.Event += HandleSpineEvent;
        _skeletonAnimation.AnimationState.Complete += HandleSpineComplete;
        _skeletonAnimation.AnimationState.Start += HandleSpineStart;
    }

    public void SetTimeScale(float timeScale)
    {
        _skeletonAnimation.timeScale = Mathf.Max(0, timeScale);
    }
    public void SetSortingLayerAndOrder(int sortingLayerID, int orderInLayer)
    {
        Renderer renderer = _skeletonAnimation.GetComponent<Renderer>();
        if (renderer != null)
        {
            string sortingLayerName = SortingLayer.IDToName(sortingLayerID);
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = orderInLayer;
        }
    }
    public void ChangeAttachment(string[] slotName, string[] attachmentName)
    {
        for (int i = 0; i < slotName.Length; i++)
        {
            Spine.Slot slot = _skeletonAnimation.skeleton.FindSlot(slotName[i]);
            if (slot != null)
            {
                Spine.Attachment attachment = _skeletonAnimation.skeleton.GetAttachment(slotName[i], attachmentName[i]);
                if (attachment != null)
                {
                    slot.Attachment = attachment;
                }
                else
                {
                    Debug.LogWarning("Attachment không tồn tại: " + attachmentName);
                }
            }
            else
            {
                Debug.LogWarning("Slot không tồn tại: " + slotName);
            }
        }

    }


}
