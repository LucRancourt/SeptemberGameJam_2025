using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class ButtonEnlarger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float _relativeScaleOnHover = 1.5f;
    [SerializeField] float _timeToScale = 0.5f;

    private Vector2 _hoverScale, _defaultScale;
    private RectTransform _rectTransform;

    private void Start()
    {

        _hoverScale = gameObject.transform.localScale * _relativeScaleOnHover;
        _defaultScale = gameObject.transform.localScale;

        _rectTransform = gameObject.GetComponent<RectTransform>();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _rectTransform.DOScale(_hoverScale, _timeToScale).SetUpdate(true);      //SetUpdate so that it works when timescale is set to 0 (in the pause menu)
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _rectTransform.DOScale(_defaultScale, _timeToScale).SetUpdate(true);
    }

    public void OnDisable()     //if the menu/button is disabled, we want it to go back to normal
    {
        _rectTransform.localScale = _defaultScale;
    }


}


