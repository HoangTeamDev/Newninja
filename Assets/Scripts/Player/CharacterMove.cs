using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
public class CharacterMove : MonoBehaviour
{
    public SpineController _spineController;
    public Character _character;
    public CharacterAuto _characterAuto;
    public bool _isRight;
    public float dir;
    public int key;
    public GameObject _PlayerAnimation;
    public int _countJump;
    public int _countSend;
    public int _countSendLeft;
    public int _countSendRight;
    public float _distanceAuto;
    [SerializeField] public Rigidbody2D _rb;
    [SerializeField] public BoxCollider2D _rbCollider;
    [SerializeField] public Transform _groundCheck;
    [SerializeField] public Transform _boxCheck;
    [SerializeField] public Transform _boxCheck2;
    [SerializeField] public LayerMask _groundLayer;
    public GameObject _curentBox;

    private void OnEnable()
    {
        switch (_PlayerAnimation.transform.localScale.x)
        {
            case > 0:
                _isRight = true;
                break;
            case < 0:
                _isRight = false;
                break;
        }

    }

    private void Update()
    {
        if (_character._isMainCharacter)
        {
            if (_spineController._isSkill||InputController.Instance._isAuto)
            {
                return;
            }
            IsBoxCheck();
            IsGrounded();
            PlayerMove();
        }

    }

    public void PlayerMove()
    {
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            _countSendRight = 0;
            _countSendLeft = 0;
            _countSend = 0;
            if (!_isRight)
            {
                PlayerRotate(1);
            }
            if (IsGrounded())
            {
                _spineController.PlayAnimation(0, true);
                _rb.velocity = new Vector2(5, 12);
                GameServer.gI().Charmove(0, (short)dir);

            }
            else
            {
                _spineController.PlayAnimation(-3, true);
            }
            return;
          
           
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            _countSendRight = 0;
            _countSendLeft = 0;
            _countSend = 0;
            if (_isRight)
            {
                PlayerRotate(-1);
            }
            if (IsGrounded())
            {
                _spineController.PlayAnimation(0, true);
                _rb.velocity = new Vector2(-5, 12);
                GameServer.gI().Charmove(1, (short)dir);
            }
            else
            {
                _spineController.PlayAnimation(-3, true);
            }
            return;
           
           
        }
        
        if ((Input.GetKeyDown(KeyCode.UpArrow)))
        {
            _countSendRight = 0;
            _countSendLeft = 0;
            _countSend = 0;
            PlayerJump();
            PlayerDoubleJump();
        }
     /*   if (InputController.Instance.GetVertical() == 1 && IsGrounded())
        {
            _spineController.PlayAnimation(0, true);
            GameServer.gI().Charmove(0, (short)dir);
            return;
            
        }*/

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
           
            PlayerMoveRight();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            _countSendRight = 0;
            _countSendLeft = 0;
            _countSend = 0;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
          
            PlayerMoveLeft();
        }
        if (!Input.anyKey)
        {
           
            PlayerNothing();

        }

        if (IsGrounded() && _spineController._currentStatus == _spineController.GetStatus(0)) { _countJump = 0; }
    }
    public void PlayerMove1(int key)//ho=0 
    {
        switch (key)
        {
            case 7:
                _spineController.PlayAnimation(-3);
                _rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
                break;
            case 0:
                _spineController.PlayAnimation(-3);
                if (!_isRight)
                {
                    PlayerRotate(1);

                }
                if (IsGrounded())
                {
                    _rb.velocity = new Vector2(5, 12);
                }
                break;
            case 1:
                _spineController.PlayAnimation(-3);
                if (_isRight)
                {
                    PlayerRotate(-1);

                }
                if (IsGrounded())
                {
                    _rb.velocity = new Vector2(-5,12);

                }
                break;
            case 2:
                if (IsGrounded())
                {
                    _spineController.PlayAnimation(-2);
                    _rb.AddForce(new Vector2(0, 9), ForceMode2D.Impulse);
                    _countJump++;
                    return;
                }
                break;
            case 3:
                _spineController.PlayAnimation(-3);
                _rb.AddForce(new Vector2(0, 9), ForceMode2D.Impulse);
                break;
            case 4:
                if (!_isRight)
                {
                    PlayerRotate(1);

                }
                if (IsGrounded())
                {
                    _spineController.PlayAnimation(-1, true);
                    _rb.velocity = new Vector2(1 * _character._Speed, _rb.velocity.y);

                }
                else
                {
                    _spineController.PlayAnimation(-2, true);
                    _rb.velocity = new Vector2(1 * 4, _rb.velocity.y);

                }
                break;
            case 5:

                if (_isRight)
                {
                    PlayerRotate(-1);

                }
                if (IsGrounded())
                {
                    _spineController.PlayAnimation(-1, true);
                    _rb.velocity = new Vector2(-1 * _character._Speed, _rb.velocity.y);

                }
                else
                {
                    _spineController.PlayAnimation(-2, true);
                    _rb.velocity = new Vector2(-1 * 4, _rb.velocity.y);

                }
                break;
            case 6:
                if (IsGrounded())
                {
                    _spineController.PlayAnimation(0, true);
                    _rb.velocity = new Vector2(0, 0);
                }
                else
                {
                    _spineController.PlayAnimation(-2, true);
                }

                break;
           
          
        }
        if (IsGrounded() && _spineController._currentStatus == _spineController.GetStatus(0)) { _countJump = 0; }
    }
    public void PlayerMoveLeft()
    {
        _countSendRight = 0;
        _countSend = 0;
        if (!InputController.Instance._isAuto)
        {
            _countJump = 0;

        }
        if (_countSendLeft == 0)
        {
            GameServer.gI().Charmove(5, (short)dir);
            _countSendLeft++;
        }
        if (_isRight)
        {
            PlayerRotate(-1);

        }

        if (IsGrounded())
        {
            _spineController.PlayAnimation(-1, true);
            _rb.velocity = new Vector2(-1 * _character._Speed, _rb.velocity.y);

        }
        else
        {
            _spineController.PlayAnimation(-2, true);
            _rb.velocity = new Vector2(-1 * 4, _rb.velocity.y);

        }
    }
    public void PlayerMoveRight()
    {
        _countSend = 0;
        _countSendLeft = 0;
        if (!InputController.Instance._isAuto)
        {
            _countJump = 0;

        }
        if (_countSendRight == 0)
        {
            GameServer.gI().Charmove(4, (short)dir);
            _countSendRight++;
        }

        if (!_isRight)
        {
            PlayerRotate(1);

        }

        if (IsGrounded())
        {
            _spineController.PlayAnimation(-1, true);
            _rb.velocity = new Vector2(1 * _character._Speed, _rb.velocity.y);

        }
        else
        {
            _spineController.PlayAnimation(-2, true);
            _rb.velocity = new Vector2(1 * 4, _rb.velocity.y);

        }
    }
    public void PlayerJump()
    {
        if (IsGrounded() && _countJump == 0)
        {
            _spineController.PlayAnimation(-2);
            _rb.AddForce(new Vector2(0, 9), ForceMode2D.Impulse);
            GameServer.gI().Charmove(2, (short)dir);
            _countJump++;
            return;
        }
    }
    public void PlayerNothing()
    {
        _countSendRight = 0;
        _countSendLeft = 0;
        if (_countSend < 5)
        {
            if (IsGrounded())
            {
                GameServer.gI().Charmove(6, (short)dir);
                _countSend++;
                _rb.velocity = new Vector2(0, 0);
                _spineController.PlayAnimation(0, true);
            }
            else
            {
                _countSend = 0;
                _spineController.PlayAnimation(-2, true);

            }

        }
    }
    public void PlayerDoubleJump()
    {
        if (_countJump == 1 && !IsGrounded())
        {
            _spineController.PlayAnimation(-3, true);
            _rb.AddForce(new Vector2(0, 9), ForceMode2D.Impulse);
            GameServer.gI().Charmove(3, (short)dir);
            _countJump++;
        }

        return;
    }

  
    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(_groundCheck.position, Vector2.down, 0.3f, _groundLayer);
        if (hit.collider != null)
        {
            _curentBox = hit.collider.gameObject;
            return true;
        }
        else
        {
            _curentBox = null;
            return false;
        }
       
    }

    public bool IsBoxCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(_boxCheck.position, Vector2.up, 9f, _groundLayer);
        Debug.DrawRay(_boxCheck.position, Vector2.up * 9f, Color.red);
        return hit.collider != null&& hit.collider.gameObject==_characterAuto._targetMove;
        
      
       
    }
    public bool IsBoxCheck2()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(_boxCheck2.position, Vector2.right*dir, 2f, _groundLayer);
        Debug.DrawRay(_boxCheck2.position, Vector2.right * dir, Color.red);
        return hit.collider != null;



    }



    public void PlayerRotate(float index)
    {
        _isRight = !_isRight;
        Vector3 scale = _PlayerAnimation.transform.localScale;
        scale.x *= -1;
        _PlayerAnimation.transform.localScale = scale;
    }
}
