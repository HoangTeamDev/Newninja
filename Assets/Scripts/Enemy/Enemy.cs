using System.Linq;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using System.Collections;
using System;
using DG.Tweening.Core.Easing;



public class Enemy : MonoBehaviour, ICharacter
{
    [Header("Start")]   
    public GameObject _pointPopUP; 
    public GameObject _arrow; 
    public int _point;
    [Header("Info")]
    public int _idEnemy;
    public int _indexEnemy;
    public string _nameEnemy;
    public float _xEnemy;
    public float _yEnemy;
    public long _hpEnemy;
    public long _maxHp;
    public int _atk;
    public bool _isMobMe;
    public bool _isStunned;
    public bool _KhangLua;
    public bool _KhangGio;
    public bool _KhangBang;
    public int _lever;
    public long _maxExp;
    public int _status;
    public int _levelBoss;
    public bool _isBoss;
    public bool _isDontMove;
    public int _level;
    public bool _isDie;
    public int _type;
    public float _distanceMove;
    public int _speed;
    public GameObject _Slash;
    [Header("Script")]
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private EnemyMove _enemyMove;
    [SerializeField] private Transform _attackPoint;  
    public EnemySO _data;
    public Character _character;
    public void EnemyOnLoad()
    {        
        _enemyAnimation.ChangeSkeletonData(_idEnemy);
    }
    public void EnemyRespawn(long hp,int level,bool isboss)
    {
        this._hpEnemy=hp;
        this._level = level;
        this._isBoss=isboss;
        gameObject.SetActive(true);
    }
   
    private void OnEnable()
    {
       /* if (gameObject == TargetController.Instance._targetNow)
        {
            Character character = GameController.Instance._mainCharacter.GetComponent<Character>();
            character.playerAnimation._isSkill = false;
            InputController.Instance.canPress = true;
        }*/
        SetActiveArrow(false);      
    }
    public void UpdateStatusArrow(int id)
    {
        SpriteRenderer spriteRenderer = _arrow.GetComponent<SpriteRenderer>();
        

    }
   
    private void FixedUpdate()
    {
        try
        {
        }
        catch (Exception)
        {
           
        }
        
    }
 
    public void SetTarGet()
    {


    }


    private void OnDisable()
    {
        //_arrow.SetActive(false);

        if (TargetController.Instance._targetNow == gameObject)
        {
            if (GameController.Instance?._mainCharacter == null)
                return;

            if (GameController.Instance._mainCharacter.TryGetComponent(out CharacterMove characterMove))
            {
               // characterMove._rigidbody2.gravityScale = 3;              
            }
            //TargetController.Instance.targetNow = null;

            if (TargetController.Instance._targetMe == gameObject)
            {
                TargetController.Instance._targetMe = null;
            }
        }
    }

    private void OnMouseDown()
    {
        if (InputController.Instance.IsPointerOverUI())
            return;
        Selectobject();
    }
    public void Selectobject()
    {

        InputController.Instance._clickPosition = transform.position;

        if (TargetController.Instance._targetNow == gameObject)
        {
            _Slash.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
           _Slash.SetActive(true);
            Invoke("HideSplash", 0.1f);
            InputController.Instance._isAuto = true;
            InputController.Instance._isAttack = true;
            InputController.Instance._isDoubleEnter = true;
        }
        else
        {
            TargetController.Instance.TarGetNow(gameObject);
           
        }

    }
    
    public void SetAttackTarget(Transform target, long damage)
    {
        _enemyAnimation._isAttack = true;
        if (transform.position.x <= target.position.x)
        {
            if (!_enemyMove._isRight)
            {
                _enemyMove._isRight = true;
                _enemyMove.EnemyRorate(1);
            }
        }
        else
        {
            if (_enemyMove._isRight)
            {
                _enemyMove._isRight = false;
                _enemyMove.EnemyRorate(-1);
            }
        }
        float distance = Vector2.Distance( transform.position, target.position);
        _enemyAnimation._isAttack=true;
        if (distance <= 1)
        {
            _enemyAnimation.PlayAnimation(4);
            _character=target.GetComponent<Character>();
            if ( _character != null )
            {
                _character.TakeDamage(damage, false);
            }
        }
        else
        {
            _enemyAnimation.PlayAnimation(5);
            EnemyAttack(target.gameObject, damage);
        }
             
    }
   
    public void EnemyAttack(GameObject target,long dame)
    {
        _enemyAnimation._isAttack = false;
        GameObject bullet = PoolingContronller.Instance.GetBulletEnemykPool();
        BulletEnemy bulletEnemy=bullet.GetComponent<BulletEnemy>();
        bulletEnemy._dame = dame;
        bulletEnemy._player = target;
    }

  
    public void SetActiveArrow(bool value)
    {
        if (value)
        {
            //_arrow.SetActive(value);
           // _healthBar.SetShow(value);
            //PlayerSelect();

        }
        else
        {
           // _arrow.SetActive(value);
            //_healthBar.SetShow(value);

        }

    }
    public string GetName()
    {
        return _nameEnemy;
    }

    public int GetId() { return _idEnemy; }
    public void TakeDamage(long damage, bool value)
    {
        if (damage == -1)
        {
            return;
        }
        else
        {
            _enemyAnimation.PlayAnimation(2);
            CreateTextPopUp(damage, "-", value);
        }
        if (_isDie)
            return;

       
    }
    public void CreateTextPopUp(long number, string t, bool crit)
    {
        GameObject pos = null;
        if (number != 0)
        {
            if (!crit)
            {
                pos = PoolingContronller.Instance.GetTextPopup();
            }
            else
            {
                pos = PoolingContronller.Instance.GetTextPopupCrit();
            }
        }
        else
        {
            pos=PoolingContronller.Instance.GetTextPopupMiss();
        }
       
        switch (_point)
        {
            case 0:
                pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y);

                break;
            case 1:
                pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y + 0.3f);

                break;
            case 2:
                pos.transform.position = new Vector2(_pointPopUP.transform.position.x, _pointPopUP.transform.position.y + 0.4f);

                break;
        }
        _point++; if (_point > 2) { _point = 0; }
        TextMeshProUGUI textMeshPro = pos.GetComponentInChildren<TextMeshProUGUI>();      
        if (number <= 0)
        {
            textMeshPro.text = "Miss";                    
            PoolingContronller.Instance.ReturnTextPopupMiss(pos);           
        }
        else
        {
            textMeshPro.text = t + number;
            if (!crit)
            {
                PoolingContronller.Instance.ReturnTextPopup(pos);

            }
            else
            {
                PoolingContronller.Instance.ReturnTextPopupCrit(pos);
            }
        }
       

    }

    public void SetMaxHealth(long maxHeath)
    {
        _hpEnemy = maxHeath;
        _healthBar.ChangeHealth(maxHeath, maxHeath);
    }
    public void SetHealth(long currentHeath)
    {
        _healthBar.ChangeHealth(_hpEnemy, currentHeath);
    }
    public void EnemyDie()
    {
        if (TargetController.Instance._targetNow == gameObject)
        {
            //InputController.Instance.SetMove(6);
        }
        if (_isDie)
            return;
        _isDie = true;
        _healthBar.SetShow(false);
                
    }

   
   
  
}
