using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.TextCore.Text;

public class CharacterAuto : MonoBehaviour
{
    public CharacterMove _characterMove;
    public SpineController _spineController;
    public Character _character;
    public SkillController _skillController;
    public float _xtarget;
    public float _ytarget;
    public float _distanceSkill;
    // Update is called once per frame
    public GameObject _targetMove;
    public bool _moveJump;
    public bool _isStartMove;
    public bool _isBox;
    public GameObject _targetTo;
    public BoxCheck _boxCheck;
    private void FixedUpdate()
    {
        if (_character._isMainCharacter)
        {
            AutoMove();

        }
    }
    GameObject FindNearestObject()
    {
        if (_characterMove._curentBox == _targetTo && transform.position.x == _xtarget)
        {
            return null;
        }

        // Lấy tất cả các collider trong phạm vi quét
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 50f, _characterMove._groundLayer);

        GameObject nearestObject = null;
        float shortestDistanceY = Mathf.Infinity;
        float shortestDistanceX = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {

            if (collider.gameObject == _characterMove._curentBox || collider.gameObject.tag == "Ground")
            {
                continue;
            }


            float distanceY = Mathf.Abs(collider.transform.position.y - transform.position.y);
            float distanceX = Mathf.Abs(collider.transform.position.x - transform.position.x);


            if (collider.transform.position.y > transform.position.y)
            {
                if (distanceY < shortestDistanceY)
                {
                    shortestDistanceY = distanceY;
                    shortestDistanceX = distanceX;
                    nearestObject = collider.gameObject;
                }
            }

            else
            {
                if (distanceX < shortestDistanceX)
                {
                    shortestDistanceX = distanceX;
                    nearestObject = collider.gameObject;
                }
            }
        }

        return nearestObject;
    }

    public void AutoMove()
    {
        if (InputController.Instance._isAuto && !_spineController._isSkill && !_character._isDie)
        {

            if (!_isStartMove)
            {
                if (TargetController.Instance._targetNow == null)
                {
                    _xtarget = InputController.Instance._clickPosition.x;
                    _ytarget = InputController.Instance._clickPosition.y;
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(_xtarget, _ytarget), Vector2.down, 50f, _characterMove._groundLayer);
                    if (hit.collider != null)
                    {
                        if (hit.collider.tag == "Box2")
                        {

                            _xtarget = hit.point.x;
                            _ytarget = hit.point.y;
                            _moveJump = true;
                            _targetTo = hit.collider.gameObject;                         
                           
                        }
                        else
                        {
                            _moveJump = false;
                        }

                        _isStartMove = true;
                    }
                }
                else
                {

                }
            }
            if (_moveJump)
            {
                AutoMoveJump();
            }
            else
            {
                AutoMoveDontJump();

            }
        }

    }

    public void MoveLeft()
    {
        _characterMove.PlayerMoveLeft();
        //_characterMove._countJump = 0;

    }
    public void MoveRight()
    {
        _characterMove.PlayerMoveRight();
        //_characterMove._countJump = 0;
    }
    public void AutoMoveJump()
    {
       
        if (_xtarget == transform.position.x)
        {
            _characterMove._countSendLeft = 0;
            _characterMove._countJump = 0;
            InputController.Instance._isAuto = false;
            InputController.Instance._isAttack = false;
            InputController.Instance._isDoubleClick = false;
            _isStartMove = false;
            _characterMove.PlayerNothing();
        }
        if (_ytarget != transform.position.y || _xtarget != transform.position.x)
        {
            if (_targetMove == null)
            {
               
                
                
                if (_targetMove == null)
                {
                    _characterMove._countSendLeft = 0;
                    _characterMove._countSendRight = 0;
                    _characterMove._countJump = 0;
                    InputController.Instance._isAuto = false;
                    InputController.Instance._isAttack = false;
                    InputController.Instance._isDoubleClick = false;
                    _isStartMove = false;
                    _moveJump = false;
                    _characterMove.PlayerNothing();
                }

            }
            else
            {
                float x = Mathf.Abs(transform.position.x - _targetMove.transform.position.x);
                float y = Mathf.Abs(transform.position.y - (_targetMove.transform.position.y + 1f));
                if (x > 0.1)
                {
                    if (_characterMove._countJump == 0 && _characterMove.IsBoxCheck() && _characterMove.IsGrounded())
                    {
                        _spineController.PlayAnimation(-2, true);

                        _characterMove._rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
                        _characterMove._countJump++;
                        GameServer.gI().Charmove(7, (short)_characterMove.dir);
                        _characterMove._countSendLeft = 0;
                        _characterMove._countSendRight = 0;

                    }
                    else
                    {
                        if (_targetMove.transform.position.x > transform.position.x)
                        {

                            MoveRight();
                        }
                        if (_targetMove.transform.position.x < transform.position.x)
                        {

                            MoveLeft();
                        }
                    }
                    if (_characterMove._curentBox == _targetTo && Mathf.Abs(_xtarget - transform.position.x) < 0.2f)
                    {
                        _characterMove._countSendLeft = 0;
                        _characterMove._countJump = 0;
                        _characterMove._countSendRight = 0;
                        InputController.Instance._isAuto = false;
                        InputController.Instance._isAttack = false;
                        InputController.Instance._isDoubleClick = false;
                        _isStartMove = false;
                        _moveJump = false;
                        _characterMove.PlayerNothing();
                    }

                }
                else
                {
                    _characterMove._countJump = 0;
                    _targetMove = null;
                }



            }

        }


    }
    public void AutoMoveDontJump()
    {
        float x = Mathf.Abs(transform.position.x - _xtarget);
        float y = Mathf.Abs(transform.position.y - _ytarget);
        if (x > 0.1f)
        {
            if (transform.position.x > _xtarget)
            {

                if (_characterMove.IsBoxCheck2())
                {
                    if (_characterMove._countJump == 0 && _characterMove.IsGrounded())
                    {
                        _spineController.PlayAnimation(-2, true);

                        Debug.Log("okeda");
                        _characterMove._rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
                        _characterMove._countJump++;
                        GameServer.gI().Charmove(7, 1);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    Debug.Log("okeda2");
                    //Invoke("MoveLeft", 0.1f);
                    MoveLeft();
                }

            }
            if (_xtarget > transform.position.x)
            {
                if (_characterMove.IsBoxCheck2())
                {
                    if (_characterMove._countJump == 0 && _characterMove.IsGrounded())
                    {
                        _spineController.PlayAnimation(-2, true);

                        Debug.Log("okeda");
                        _characterMove._rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
                        _characterMove._countJump++;
                        GameServer.gI().Charmove(7, 1);
                    }
                    else
                    {
                        return;
                    }

                }
                else
                {
                    Debug.Log("okeda3");
                    //Invoke("MoveRight", 0.1f);
                    MoveRight();
                }
            }
        }
        else
        {

            _characterMove._countSendLeft = 0;
            _characterMove._countJump = 0;
            InputController.Instance._isAuto = false;
            InputController.Instance._isAttack = false;
            InputController.Instance._isDoubleClick = false;
            _isStartMove = false;
            _characterMove.PlayerNothing();

        }
        /* if (y > 0.1f)
         {
             if (transform.position.y >= _ytarget)
             {
                 if (x > 0.1f)
                 {
                     if (_xtarget < transform.position.x)
                     {
                         _characterMove.PlayerMoveLeft();
                     }
                     else
                     {
                         _characterMove.PlayerMoveRight();
                     }
                 }
                 else
                 {
                     _characterMove._countSendLeft = 0;
                     _characterMove._countJump = 0;
                     InputController.Instance._isAuto = false;
                     InputController.Instance._isAttack = false;
                     InputController.Instance._isDoubleClick = false;
                     _isStartMove = false;
                     _characterMove.PlayerNothing();
                 }
             }
             else
             {




             }

         }
         else
         {
             _characterMove._countSendLeft = 0;
             _characterMove._countJump = 0;
             InputController.Instance._isAuto = false;
             InputController.Instance._isAttack = false;
             InputController.Instance._isDoubleClick = false;
             _isStartMove = false;
             _characterMove.PlayerNothing();
         }*/
    }
}
