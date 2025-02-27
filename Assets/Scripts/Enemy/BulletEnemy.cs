using System.Collections;
using System.Collections.Generic;

using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletEnemy : MonoBehaviour
{
    public GameObject _player;
    public Rigidbody2D _rb;
    public long _dame;
    public GameObject _blast;
    void Update()
    {

        if (_player != null && _player.activeInHierarchy)
        {
            Vector3 direction = (new Vector3(_player.transform.position.x, _player.transform.position.y + 0.5f, _player.transform.position.z) - transform.position).normalized;
            _rb.velocity = direction * 15;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));


            float num1 = (float)Mathf.Abs(transform.position.x - _player.transform.position.x);
            float num2 = (float)Mathf.Abs(transform.position.y - _player.transform.position.y);
            if (num1 <= 0.5 && num2 <= 0.5)
            {     
                Character character = _player.GetComponent<Character>();
                if (character != null)
                {
                    character.TakeDamage(_dame, false);
                }
                PoolingContronller.Instance.ReturnKame(gameObject);
                //_blast = PoolingContronller.Instance.GetKameNoPool();
                /*LoadNo loadNo = blast.GetComponent<LoadNo>();
                if (loadNo != null)
                {
                    loadNo.LoadAnimation(loadAnimationKameSkill.character._plannet, type);
                }*/
                /*blast.transform.position = new Vector2(target.transform.position.x, target.transform.position.y + .7f);
                blast.transform.SetParent(target.transform);*/
               
               

            }

        }
        else
        {
            PoolingContronller.Instance.ReturnKame(gameObject);
        }

        

    }
    
   
}
