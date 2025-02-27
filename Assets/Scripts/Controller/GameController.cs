using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GameController : Singleton<GameController>
{
    [Header("Controller")]
    public GameObject _mainCharacter;
    public List<GameObject> _enemyInMap;
    public List<GameObject> _itemInMap;
    public List<GameObject> _npcInMap;
    public List<GameObject> _playerInMap;
    public List<Enemy> _listEnemySend;
    public List<Character> _listCharacterSend;
    public GameObject _MainDataGame;
    public int _skillSellect;
    [Header("Map")]
    public int _idMap;
     public readonly ConcurrentDictionary<int, MovePoint> MovePoints = new ConcurrentDictionary<int, MovePoint>();
    [SerializeField] public GameObject _mapCurrent;
    [SerializeField] private GameObject _playerPref;
    [SerializeField] private Character _character;
    public List<Transform> _lisbox;
    public void DoFixedUpdate()
    {

    }
    
    public void DoUpdate()
    {
        foreach (MovePoint move in MovePoints.Values)
        {
            _character=null;
            _character = GameData.Instance.FindPlayerInMap(move._id);
            if (_character != null)
            {
               
                if (move._key != -1000)
                {
                    _character._CharacterMove.PlayerMove1(move._key);
                    Debug.Log("CharMove:" + move._key);
                    if (move._key != 5 && move._key != 4)
                    {
                        if (Mathf.Abs(move._x - _character.gameObject.transform.position.x) > 0.3f || Mathf.Abs(move._y - _character.gameObject.transform.position.y) > 0.3f)
                        {
                            _character.gameObject.transform.position = new Vector2(move._x, move._y);
                        }
                        move._key = -1000;
                    }
                   
                }



            }
          
        }
    }
    private void Start()
    {
        Time.timeScale = 1;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.SetResolution(1100, 600, fullscreenMode: FullScreenMode.Windowed);

        Physics2D.IgnoreLayerCollision(6, 6);
        Physics2D.IgnoreLayerCollision(6, 8);
        Physics2D.IgnoreLayerCollision(6, 9);
        Physics2D.IgnoreLayerCollision(6, 10);
        Physics2D.IgnoreLayerCollision(8, 10);
        PoolingContronller.Instance.CreateAllPool();
        if (_mainCharacter == null)
        {
            _mainCharacter = Instantiate(_playerPref, transform);
            _mainCharacter.SetActive(false);
            GameData.Instance._DataMain = _mainCharacter.GetComponent<Character>();
            GameData.Instance._DataMain._isMainCharacter = true;
        }
    }
    public Sprite LoadSprite()
    {
        ICharacter character = TargetController.Instance._targetNow.GetComponent<ICharacter>();
        return Resources.Load<Sprite>($"Icon/{character.GetId()}");

    }
    public void LoadDaTaMap()
    {
        if (_mapCurrent != null) { Destroy(_mapCurrent); _mapCurrent = null; }
        _idMap = GameData.Instance._titleMapData._mapID;
        _mapCurrent = Instantiate(Resources.Load<GameObject>("Map/Map_ID_" + _idMap), _MainDataGame.transform);
        BackGroundMove backGroundMove=_mapCurrent.GetComponent<BackGroundMove>();
        _lisbox=backGroundMove._list;
    }
   
    public void CleanMap()
    {
        _mainCharacter.SetActive(false);
        Destroy(_mapCurrent);_mapCurrent = null;
        foreach(GameObject data in _playerInMap)
        {
            PoolingContronller.Instance.ReturnPlayer(data);
        }
        foreach (GameObject data in _enemyInMap)
        {
            PoolingContronller.Instance.ReturnMonster(data);
        }
        foreach (GameObject data in _npcInMap)
        {
            PoolingContronller.Instance.ReturnNPC(data);
        }
        foreach (GameObject data in PoolingContronller.Instance._PlayerMainPool.transform)
        {
            data.SetActive(false);
        }
        foreach (Transform data in PoolingContronller.Instance.boxMainPool.transform)
        {
            data.gameObject.SetActive(false);
        }
        foreach (GameObject data in _itemInMap)
        {
            PoolingContronller.Instance.ReturnItem(data);
        }
        TargetController.Instance._targetNow = null;
        GameData.Instance._CharacterInMap.Clear();
        GameData.Instance._enemyInMap.Clear();
        GameData.Instance._npcInmap.Clear();
        GameData.Instance._itemInMap.Clear();
        MovePoints.Clear();
    }
}
