using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TargetController : Singleton<TargetController>
{   
    [Header("Player")]
    public GameObject _targetNow;
    public GameObject _targetMe;
    public GameObject _targetchallenge;
    private void LateUpdate()
    {
        if (_targetchallenge != null && !_targetchallenge.activeInHierarchy)
        {
            _targetchallenge=null;
            //GameController.Instance.playerPK = null;
        }
        if (_targetNow != null)
        {
            if (!_targetNow.activeInHierarchy)
            {
                _targetNow = null;
                return;
            }
            if (_targetchallenge != null) { return; }
          /*  if (targetNow.tag == "PlayerB")
            {
                Character character1 = targetNow.GetComponent<Character>();
                if (character1.idFlag == 8) { return; }
            }*/
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                Vector3 viewportPoint = mainCamera.WorldToViewportPoint(_targetNow.transform.position);
                bool isInView = viewportPoint.z > 0 && viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1;

                if (!isInView)
                {
                    ICharacter character = _targetNow.GetComponent<ICharacter>();
                    if (character != null)
                    {
                        character.SetActiveArrow(false);
                    }
                    _targetNow = null;
                }
            }
        }
        if(_targetMe!=null /*&& !InputController.Instance.isAuto*/)
        {
            /*float distance = Vector2.Distance(GameController.Instance.mainChar.transform.position, targetMe.transform.position);
            if (distance > 1)
            {
                ICharacter character = targetMe.GetComponent<ICharacter>();
                if (character != null)
                {
                    character.SetActiveArrow(false);
                }
                targetMe = null;
            }*/
        }


    }
  
    public void TarGetChallenge(GameObject target)
    {
        if (_targetMe != null)
        {
            ICharacter character = _targetMe.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(false);
                _targetMe = null;
            }
        }
        if (_targetNow != null)
        {
            ICharacter character = _targetNow.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(false);
            }
        }
        _targetNow = target;
        _targetchallenge=target;
        if (_targetNow != null)
        {
            ICharacter character = _targetNow.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(true);
            }
        }

    }
    public void TargetMe(GameObject target)
    {
        if (_targetchallenge != null)
        {
            return;
        }
        _targetMe = target;
        ICharacter character = _targetMe.GetComponent<ICharacter>();
        if (character != null)
        {
            character.SetActiveArrow(true);
        }

    }
    public void TarGetNow(GameObject target)
    {
        /*if (targetchallenge != null && GameController.Instance.mainChar.GetComponent<Character>().typePK!=0)
        {
            return;
        }*/
        if (_targetMe != null)
        {
            ICharacter character=_targetMe.GetComponent<ICharacter>();
            if(character != null)
            {
                character.SetActiveArrow(false);
                _targetMe = null;
            }
        }
        if (_targetNow != null)
        {
            ICharacter character = _targetNow.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(false);
            }
        }
        _targetNow = target;
        if (_targetNow != null)
        {
            ICharacter character = _targetNow.GetComponent<ICharacter>();
            if (character != null)
            {
                character.SetActiveArrow(true);
            }
           
        }
        

    }

   
}
