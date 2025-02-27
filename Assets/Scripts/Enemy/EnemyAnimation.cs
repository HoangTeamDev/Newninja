using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [Header("Spine Components")]
    public SkeletonAnimation _skeletonAnimation;
    public EnemyMove _enemyMove;
    public Enemy _enemy;
    public string _currentStatus;
    public bool _isAttack;
    private void Start()
    {
        _skeletonAnimation.Initialize(true);
        _skeletonAnimation.AnimationState.Event += HandleSpineEvent;
        _skeletonAnimation.AnimationState.Complete += HandleSpineComplete;
        _skeletonAnimation.AnimationState.Start += HandleSpineStart;
        PlayAnimation(0, true);
    }
    private string GetStatus(int id)
    {
        switch (id)
        {
            case 0:
                return EnemyAnimationState.Enemy_idle.ToString();
            case 1:
                return EnemyAnimationState.Enemy_move.ToString();

            case 2:
                return EnemyAnimationState.Enemy_block.ToString();
            case 3:

                return EnemyAnimationState.Enemy_die.ToString();
            case 4:
                return EnemyAnimationState.Enemy_attack1.ToString();

            case 5:
                return EnemyAnimationState.Enemy_attack2.ToString();

        }
        return null;
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
    private void HandleSpineStart(Spine.TrackEntry trackEntry)
    {

    }
    private void HandleSpineComplete(Spine.TrackEntry trackEntry)
    {

        if (trackEntry.Animation.Name == GetStatus(4))
        {
            _isAttack = false;
           
        }
    }
    private void HandleSpineEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
      
        Debug.Log("EnemyEvent:" + e.Int);
        if (e.Data.Name == "EnemyEvent")
        {
            Debug.Log("EnemyEvent:" + e.Int);
            switch (e.Int)
            {
                case 0:
                    break;

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

    public void ChangeSkeletonData(int id)
    {       
        _skeletonAnimation.AnimationState.Event -= HandleSpineEvent;
        _skeletonAnimation.AnimationState.Complete -= HandleSpineComplete;
        _skeletonAnimation.AnimationState.Start -= HandleSpineStart;
        _skeletonAnimation.skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Enemy/"+_enemy._idEnemy);
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
