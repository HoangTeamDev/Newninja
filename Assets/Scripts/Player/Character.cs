using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
[Serializable]
public class Character : MonoBehaviour, ICharacter
{
    [Header("Target")]
    public GameObject _enemyTarget;
    public CharacterMove _CharacterMove;
    public Button _ButtonInfoPlayer;
    public int _indexTarget;
    
    public bool _isBoss; 
    public bool _isMainCharacter;
    public bool _isDie;
    public int _gender;//nam nu
    public int _hair;//1-6
    public int _bag;
    public int _level;
    public int _pk;
    public int _playerId;
    public string _namePlayer;
    public int _status;
    public int _typePK;
    public long _exp;
    public long _expCurent;
    public long _expToUp;
    public long _hp;
    public long _mp;
    public long _hpFull;
    public long _mpFull;
    public long _damageFull;
    public long _defFull;
    public int _critFull;
    public long _originHP;
    public long _originMP;
    public long _originDamege;
    public int _originDef;
    public int _originCrit;
    public long _InternalForce;
    public long _Precision;
    public float _Speed;
    public int _KhangHoa;
    public int _KhangBang;
    public int _KhangGio;
    public int _mounthid;
    public int _teleport;
    public long _xu;
    public long _luong;
    public long _yen;
    public float _x;
    public float _y;
    [Header("Skill")]
    public int[] _levelSkill = new int[8];
    public int _point;
    public int _currentIndexPlayer;
    public GameObject _arrow;
    public SpineController _SpineController;
    public event EventHandler _OnSelectEvent;
    public GameObject _pointPopUP;
    public GameObject _PanelText;
    public TextMeshProUGUI _textChat;
    public Image _image;
    public GameObject _PanelNoti;
    [SerializeField] public List<Sprite> _arrowList;
    public GameObject _PannelName;
    public TextMeshProUGUI _NameUIPlayer;
    public Image _iconPlayer;
    public Sprite[] _iconClan;
    public GameObject _statusPoint;
     public Task _taskMaint;
    public List<Skill> _Skills =new List<Skill>();
    public Clan _clan;
    public Skill _myskill;
    public int _bagLenght;
    public List<Item> _listItemBag;
    public List<Item> _listItemBox;
    public List<Item> _ListItemBody;
    private void Start()
    {
        OnStart();
      
    }
    private void OnEnable()
    {
       
       
    }
    public void ResetCharacterData()
    {
            
        _indexTarget = 0;
        _isBoss = false;
        _isMainCharacter = false;
        _isDie = false;
        _gender = 0;
        _hair = 0;
        _bag = 0;
        _level = 1;
        _pk = 0;
        _playerId = 0;
        _namePlayer = string.Empty;
        _status = 0;
        _typePK = 0;
        _exp = 0;
        _hp = 0;
        _mp = 0;
        _hpFull = 0;
        _mpFull = 0;
        _damageFull = 0;
        _defFull = 0;
        _critFull = 0;
        _originHP = 0;
        _originMP = 0;
        _originDamege = 0;
        _originDef = 0;
        _originCrit = 0;
        _InternalForce = 0;
        _Precision = 0;
        _Speed = 0;
        _KhangHoa = 0;
        _KhangBang = 0;
        _KhangGio = 0;
        _mounthid = 0;
        _teleport = 0;
        _xu = 0;
        _luong = 0;
        _yen = 0;
        _x = 0;
        _y = 0;

        Debug.Log("Character data has been reset.");
    }
    private void OnStart()
    {
        if (_isMainCharacter)
        {
                 
            _SpineController._isSkill = false;
        }

    }
  
    public void SetPos(float x, float y)
    {
        _x = x; _y = y;
        transform.position=new Vector3(x,y,0);
    }
    private void Update()
    {
        
    }

    //ChatNoti
    /*  public void ChatNoti(string chat)
      {
          MessNhanVat.textMeshProUGUI.text = chat;
          PanelNoti.SetActive(true);
      }*/
    //LoadBoss

    //LoadFlag
    /*public void LoadFlag(int id)
    {
        if (id > 0)
        {
            idFlag = id;
            isFlag = true;
            SpriteRenderer spriteRenderer = flag.GetComponent<SpriteRenderer>();
            int index = indexFlag.IndexOf(idFlag);
            spriteRenderer.sprite = Resources.Load<Sprite>("Gson/ItemSprite/Item" + Flagid[index]);
            flag.SetActive(true);
        }
        else
        {
            isFlag = false;
            idFlag = id;
            flag.SetActive(false);
        }

    }*/
    // tự động tìm mục tiêu ở gần
    void DetectTargets()
    {
        if (TargetController.Instance._targetNow != null)
        {
            Character character = TargetController.Instance._targetNow.GetComponent<Character>();
            if (character != null)
            {
                if (character._typePK != 0)
                {
                    return;
                }
            }
        }
        if (TargetController.Instance._targetNow == null && TargetController.Instance._targetMe == null)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1);
            var targets = hitColliders
                .Where(collider => collider.CompareTag("Character"))
                .OrderBy(collider => Vector2.Distance(transform.position, collider.transform.position))
                .ToArray();

            if (targets.Length > 0)
            {

                float distance = 2;
                for (int i = 0; i < targets.Length; i++)
                {

                    if (distance > Vector2.Distance(transform.position, targets[i].transform.position))
                    {
                        _indexTarget = i;
                        distance = Vector2.Distance(transform.position, targets[i].transform.position);
                    }

                }
                TargetController.Instance.TargetMe(targets[_indexTarget].gameObject);

            }
        }
    }

    public void ChangleTargetWith()
    {
        TargetController.Instance.TarGetNow(FindTargetInMap());
    }
    //FindTarget by Tab
    public GameObject FindTargetInMap()
    {
        if (TargetController.Instance._targetMe != null)
        {
            ICharacter character = TargetController.Instance._targetMe.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(false);
            }
            TargetController.Instance._targetMe = null;
        }

        List<GameObject> otherInmap = new List<GameObject>();
        Camera mainCamera = Camera.main;

       /* foreach (var kvp in GameController.Instance.playerinmap)
        {
            if (kvp.Value != null)
            {
                Character character = kvp.Value.GetComponent<Character>();
                if (!character.isMainCharacter && IsInView(mainCamera, kvp.Value))
                {
                    otherInmap.Add(kvp.Value);
                }
            }
        }

        foreach (var kvp in GameController.Instance.enemyInMap)
        {
            Enemy enemy = kvp.GetComponent<Enemy>();
            Mob mob = GameData.Instance.lstMob[enemy.indexEnemy];

            if (IsInView(mainCamera, kvp) && mob.hp > 0)
            {
                otherInmap.Add(kvp);
            }
        }

        foreach (var kvp in GameController.Instance.npcInMap)
        {
            if (IsInView(mainCamera, kvp))
            {
                otherInmap.Add(kvp);
            }
        }

        foreach (var kvp in GameController.Instance.itemInMap)
        {
            if (IsInView(mainCamera, kvp))
            {
                otherInmap.Add(kvp);
            }
        }*/

        GameObject num11 = null;
        if (_currentIndexPlayer < otherInmap.Count)
        {
            num11 = otherInmap[_currentIndexPlayer];
            _currentIndexPlayer++;
        }
        else
        {
            _currentIndexPlayer = 0;
            num11 = otherInmap[_currentIndexPlayer];
        }

        return num11;
    }
    // kiểm tra đối tượng trong màn hình
    private bool IsInView(Camera camera, GameObject obj)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(obj.transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
    //interface Icharacter
    public void SetTarGet()
    {
       
       /* Character character = GameController.Instance.mainChar.GetComponent<Character>();
        if (GameController.Instance.skillSellect is 9)
        {
            //character.characterMove.AttackToClick();          
            return;
        }
        if (character.typePK != 0 && typePK != 0 || character.idFlag != idFlag && idFlag != 0 || character.idFlag is 8 && idFlag != 0 || idFlag is 8 && character.idFlag != 0||typePK==5)
        {
            //character.characterMove.AttackToClick();
            ButtonInfoPlayer.gameObject.SetActive(false);
            return;
        }
        if (typePK == 0)
        {
            ButtonInfoPlayer.gameObject.SetActive(true);
        }
*/
    }
    public void DoFixedUpdate()
    {
        UpdateTypePK();
        UpdatePlayerTarget();
       // if (characterMove != null) { characterMove.DoFixedUpdate(); }
        if (_isMainCharacter)
        {
           
        }
        else
        {

           
        }
        if ( _isMainCharacter) { DetectTargets(); }

    }
    void UpdateTypePK()
    {
        if (_typePK is 3 or 5)
        {
            _NameUIPlayer.color = Color.red;
        }
        else
        {
           // NameUIPlayer.color = isOnParty ? Color.green : Color.white;
        }
    }
    void UpdatePlayerTarget()
    {
      
        
    }
    public void DoUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangleTargetWith();
        }
        //tắt tdlt
       
      
        //di chuyển lại khi đang tdlt hay tự di chuyển
      
        //if (characterMove != null) { characterMove.DoUpdate(); }
        //if (_Move != null) { _Move.DoUpdate(); }


    }
    //-------Status
    public void TuDongLuyenTapActive(bool value)
    {      
    }
    //cài lại cooldown skill khi dùng chiêu
 /*   public void SetTimeCoolDown(int id)
    {
        if (!isMainCharacter) return;
        var skillToUse = vSkillChar.Find(s => s.Id == id);
        skillToUse.lastTimeUseThisSkill = MSystem.currentTimeMillis() + skillToUse.coolDown;
        _skillController.isSendSkill = false;
    }*/
    // bật cờ 
    public void SetActiveArrow(bool value)
    {
        if (_arrow != null)
        {
            _arrow.SetActive(value);
            _ButtonInfoPlayer.gameObject.SetActive(false);
        }
        //NameUIPlayer.text = _namePlayer;

    }
    public string GetName()
    {
        return _namePlayer;
    }
    public int GetId()
    {
        return _playerId;
    }
    public void TakeDamage(long damage, bool value)
    {
        
        if (damage != -1)
        {
            if (value)
            {
                CreateTextPopUp(damage, "-", 2);
            }
            else
            {
                CreateTextPopUp(damage, "-", 0);

            }
        }


    }
   
    public void SetStune(int time)
    {
       
       
    }
   
   
   
   
    public void CreateTextPopUp(long number, string t, int type)
    {
        //type 0: hp-red, 1 mp-blue, 2 gold-yellow, 3 ngoc-green, 4 vatpham-grey,5 tiem nang-green, 6 damecrit-
        GameObject pos = null;
        if (number == 0)
        {
            pos = PoolingContronller.Instance.GetTextPopupMiss();

        }
        else
        {
            switch (type)
            {
                case 0:
                    pos = PoolingContronller.Instance.GetTextPopup();
                    break;
                case 1:
                    pos = PoolingContronller.Instance.GetTextPopupKi();
                    break;
                case 2:
                    pos = PoolingContronller.Instance.GetTextPopupCrit();
                    break;
                case 3:
                    pos = PoolingContronller.Instance.GetTextPopupTiemNang();
                    break;
                case 4:
                    pos = PoolingContronller.Instance.GetTextPopupKi();
                    break;
                case 5:
                    pos = PoolingContronller.Instance.GetTextPopupTiemNang();
                    break;
                case 6:
                    pos = PoolingContronller.Instance.GetTextPopupCrit();
                    break;
            }
        }     
        TextMeshProUGUI textMeshProUGUI = pos.GetComponentInChildren<TextMeshProUGUI>();

        switch (t)
        {
            case "+":
                pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y + 0.5f);
                break;
            case "-":
                switch (_point)
                {
                    case 0:
                        pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y);
                        break;
                    case 1:
                        pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y + 0.2f);

                        break;
                    case 2:
                        pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y - 0.2f);

                        break;
                }
                _point++; if (_point > 2) { _point = 0; }

                break;
        }
        if (number == 0)
        {
            textMeshProUGUI.text = "Miss";
            PoolingContronller.Instance.ReturnTextPopupMiss(pos);
        }
        else
        {
            textMeshProUGUI.text = t + number;
            switch (type)
            {
                case 0:
                    PoolingContronller.Instance.ReturnTextPopup(pos);
                    break;
                case 1:
                    PoolingContronller.Instance.ReturnTextPopupKi(pos);
                    break;
                case 2:
                    PoolingContronller.Instance.ReturnTextPopupCrit(pos);
                    break;
                case 3:
                    PoolingContronller.Instance.ReturnTextPopupTiemNang(pos);
                    break;
                case 4:
                    PoolingContronller.Instance.ReturnTextPopupKi(pos);
                    break;
                case 5:
                    PoolingContronller.Instance.ReturnTextPopupTiemNang(pos);
                    break;
                case 6:
                    PoolingContronller.Instance.ReturnTextPopupCrit(pos);
                    break;
            }
        }


    }

    
    /*public AudioSource GetAudioSource()
    {
        return audioSource;
    }*/
    private void OnMouseDown()
    {
        Selectobject();
    }
// interface Icharacter
    public void Selectobject()
    {
        
    }
    public void HidePannel()
    {
        _PanelText.SetActive(false);
    }
    public void ChatZone(string text)
    {

        switch (text.Length)
        {
            case >= 80:
                text = text.Substring(0, 80);
                RectTransform rect = _image.GetComponent<RectTransform>();
                rect.sizeDelta = new Vector2(210, 130);
                RectTransform rect1 = _textChat.GetComponent<RectTransform>();
                rect1.sizeDelta = new Vector2(150, rect1.sizeDelta.y);
                break;
            case >= 20 and <= 80:
                RectTransform rect2 = _image.GetComponent<RectTransform>();
                rect2.sizeDelta = new Vector2(210, 130);
                RectTransform rect3 = _textChat.GetComponent<RectTransform>();
                rect3.sizeDelta = new Vector2(150, rect3.sizeDelta.y);
                break;
            default:
                RectTransform rect4 = _image.GetComponent<RectTransform>();
                rect4.sizeDelta = new Vector2(150, 90);
                RectTransform rect5 = _textChat.GetComponent<RectTransform>();
                rect5.sizeDelta = new Vector2(150, rect5.sizeDelta.y);
                break;
        }

        _textChat.text = text;
        _PanelText.SetActive(true);
        Invoke("HidePannel", 1);
    }


    private void OnDisable()
    {
        
        if (transform.localScale.x > 1)
        {
            transform.localScale=Vector3.one;
        }

    }
}





