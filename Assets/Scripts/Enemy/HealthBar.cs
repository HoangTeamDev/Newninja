using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_HeathBar;
    [SerializeField] private float m_TimeDrain;
    [SerializeField] private Gradient m_HeathBarGradient;

    private float _targetValue;

    

    public void ChangeHealth(float max,float current) {
        _targetValue = current / max;
       
        m_HeathBar.value = _targetValue;
        //m_HeathBar.DORewind();
        m_HeathBar.DOValue(_targetValue, m_TimeDrain).OnComplete(() => {

        }).SetEase(Ease.InSine);
    }

    public void Relase() {
        m_HeathBar.value = 1;
    }

    public void SetShow(bool isActived) {
        m_HeathBar.gameObject.SetActive(isActived);
    }

    #region Menu
    private float _maxHeath = 100f;
    private float _heath = 80f;
    private float _damage = 5f;
    [ContextMenu("Change Heath")]
    public void MenuChangeHeath() {
        _heath -= _damage;
        ChangeHealth(_maxHeath, _heath);
    }
    #endregion
}
