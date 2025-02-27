using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class ItemMap : MonoBehaviour,ICharacter
{
    public int _playerId;
    public float _x;
    public float _y;
    public int _itemMapID;
    public int _idTemplateID;
    public SpriteRenderer _spriteRenderer;
    public void Onload (int playerId, int itemMapID, short itemTemplateID, float x, float y)
    {
        this._itemMapID = itemMapID;
        this._idTemplateID = itemTemplateID;
        this._x = x;
        this._y = y;     
        this._playerId = playerId;  
        transform.position=new Vector3(x,y,0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = Resources.Load<Sprite>("Item/" + _idTemplateID);
    }
    private void OnMouseDown()
    {
        GameServer.gI().PickItem(_itemMapID);
    }
    public void TakeDamage(long damage, bool value) { }
    public string GetName() { return null; }
    public int GetId() {  return _idTemplateID; }
    public void SetTarGet() { }
    public void SetActiveArrow(bool active) { }
    public void Selectobject() { }
}
