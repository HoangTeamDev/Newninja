using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public class EnemyMove : MonoBehaviour
{
    [SerializeField] private Enemy _Enemy;
    [SerializeField] private EnemyAnimation _enemyAnimation;
    [SerializeField] private GameObject _EnemyRotate;
    [SerializeField] private Vector3[] _pointMove;
    public int _currentPoint;
    public bool _isRight;
    public float _timeStop;

    public void EnemyRorate(int id)
    {
        _isRight = !_isRight;
        Vector3 scale = _EnemyRotate.transform.localScale;
        scale.x *= -1;
        _EnemyRotate.transform.localScale = scale;
    }
    private void OnEnable()
    {
        _timeStop = 0;
        _pointMove[0] = transform.position + new Vector3(2, 0, 0);
        _pointMove[1] = transform.position - new Vector3(2, 0, 0);
        _pointMove[2] = transform.position + new Vector3(0, 2, 0);
        _EnemyRotate.transform.localScale = Vector3.one;
        _isRight = true;
        switch (_Enemy._type)
        {
            case 0:
                _currentPoint = Random.Range(0, 1);

                break;
            case 1:
                _currentPoint = Random.Range(0, _pointMove.Length);
                break;
        }
    }
    private void Update()
    {
        if (!_Enemy._isDontMove && !_enemyAnimation._isAttack)
        {
            MonterMove(_Enemy._type);

        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            EnemyKnock();
        }
    }
    public void DoUpdate()
    {
        if (!_Enemy._isDontMove && !_enemyAnimation._isAttack)
        {
            MonterMove(_Enemy._type);

        }
    }
    public void MonterMove(int id)
    {
        if (transform.position != _pointMove[_currentPoint])
        {
            _enemyAnimation.PlayAnimation(1,true);

            if (transform.position.x < _pointMove[_currentPoint].x)
            {
                _isRight = true;
                if (_EnemyRotate.transform.localScale.x < 0)
                {
                    EnemyRorate(1);
                }
            }
            else
            {
                _isRight = false;
                if (_EnemyRotate.transform.localScale.x > 0)
                {
                    EnemyRorate(-1);
                }
            }
            Vector3 targetPos = _pointMove[_currentPoint];

            transform.position = Vector3.MoveTowards(transform.position, targetPos, _Enemy._speed * Time.deltaTime);


        }
        else
        {
            if (_timeStop < 1)
            {
                _enemyAnimation.PlayAnimation(0,true);
                _timeStop += Time.fixedUnscaledDeltaTime;
            }
            else
            {
                _timeStop = 0;
                switch (id)
                {
                    case 0:
                        _currentPoint = (_currentPoint == 1) ? 0 : 1;

                        break;
                    case 1:
                        _currentPoint = Random.Range(0, _pointMove.Length);

                        break;
                }
            }
            
        }

    }
    public void EnemyKnock()
    {
        _Enemy._isDontMove = true;
        switch (_EnemyRotate.transform.localScale.x)
        {
            case > 0:
                transform.position  = new Vector2(transform.position.x - 0.5f, transform.position.y);
                _Enemy._isDontMove = false;

                break;
            case < 0:
                transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y);
                _Enemy._isDontMove = false;
                break;
        }
        if (transform.position.x > _pointMove[0].x)
        {
            transform.position = new Vector2(_pointMove[0].x - 3f, transform.position.y);
        }
        if (transform.position.x < _pointMove[1].x)
        {
            transform.position = new Vector2(_pointMove[1].x +3f, transform.position.y);
        }
    }

}
