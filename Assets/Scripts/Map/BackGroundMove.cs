

using System.Collections.Generic;
using UnityEngine;


public class BackGroundMove : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Vector2 _rootPosCamera;
     public Vector3[] _rootLayout;
    // Start is called before the first frame update
   
    private Vector2 _lastCharacterPosition;
    public bool _movey;
    public List<GameObject> _objmove;
    public List<Vector2> _offsetCamera;
    public List<Vector3> _offsetLayout;
    [SerializeField, Range(-5f, 5f)] public List<float> _parallaxEffectMultiplierX;
    [SerializeField, Range(-5f, 5f)] public List<float> _parallaxEffectMultiplierY;
    public List<Transform> _list= new List<Transform>();
    private void Awake()
    {
        _mainCamera = Camera.main;  
        for (int i=0;i< _offsetLayout.Count;i++)
        {
            _rootLayout[i]=_objmove[i].transform.position;
        }
    }
    //public CharacterMove character;

    private void Start()
    {
        //enabled = true;
    }

    private void Update()
    {    
        for(int i = 0; i < _objmove.Count; i++)
        {
            _offsetCamera[i] = (Vector2)_mainCamera.transform.position - _rootPosCamera;
            _offsetLayout[i] = new Vector2(_offsetCamera[i].x * Mathf.Abs(_parallaxEffectMultiplierX[i]), _offsetCamera[i].y * Mathf.Abs(_parallaxEffectMultiplierY[i]));
            _objmove[i].transform.position = _offsetLayout[i] + _rootLayout[i];
        }
       
        

    }
}