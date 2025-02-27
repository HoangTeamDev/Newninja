using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Npc : MonoBehaviour,ICharacter
{
    public float _x;
    public float _y;
    public int _npcId;
    public int _templateId;
    public string _nameNpc;
    public int _status;
    public SkeletonAnimation _skeletonAnimation;
    public void Onload(int id,int status,float x,float y,string name )
    {
        this._x = x;
        this._y = y;
        
        this._templateId = id;
        this._nameNpc = name;
        this._status = status;
        transform.position = new Vector3(x,y,0);
        _skeletonAnimation.skeletonDataAsset = Resources.Load<SkeletonDataAsset>("Npc/" + _templateId);
    }
    public void OnsetActive(bool value)
    {
        gameObject.SetActive(value);
    }
    public void TakeDamage(long damage, bool value) { }
    public string GetName() { return _nameNpc; }
    public int GetId() { return _npcId; }
    public void SetTarGet() { }
    public void SetActiveArrow(bool active) { }
    public void Selectobject() { }
}
