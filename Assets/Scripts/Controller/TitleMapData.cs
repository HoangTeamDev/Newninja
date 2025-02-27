using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class TitleMapData {
    public MyVector _VGo = new MyVector();
    public int _mapID;
    public sbyte _planetID;
    public int _tileID;
    public int _bgID;
    public int _typeMap;
    public string _mapName = string.Empty;
    public int _zoneID;
    public int _bgType;
    public List<Waypoint> _lstWayPoint = new List<Waypoint>();
   
    
}
[System.Serializable]
public class Waypoint {
    public float _x;

    public float _y;
    public bool _isEnter;

    public bool _isOffline;

    public string _name;

    public Waypoint(float minX, float minY, bool isEnter, bool isOffline, string name) {
        this._x = minX;
        this._y = minY;       
        this._isEnter = isEnter;
        this._isOffline = isOffline;
        this._name = name;
    }
  

   
}





