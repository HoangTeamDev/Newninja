using JetBrains.Annotations;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class BubbleText : MonoBehaviour {
    [SerializeField] private TextMeshPro _textMeshPro;
    [SerializeField] Color _damgeColor = Color.red;
    //[SerializeField] private float lifeTime = 1;
    //[SerializeField] private float hightY = 5f;
    public void OnInit(string text, Color color) {
        _textMeshPro.text = text;
        _textMeshPro.color = color;

        //_textMeshPro.transform.localPosition = Vector3.zero;
        //_textMeshPro.transform.DOLocalMoveY(10, .5f).SetEase(Ease.OutSine);
    }

    public void OnInit(string text) {
        _textMeshPro.text = text;
        _textMeshPro.color = _damgeColor;

        //_textMeshPro.transform.localPosition = Vector3.zero;
        //_textMeshPro.transform.DOLocalMoveY(10, .5f).SetEase(Ease.OutSine);
    }
}
