using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEnlarger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] float _relativeScaleOnHover = 1.5f;
    [SerializeField] float _timeToScale = 0.5f;

    private Vector2 _hoverScale, _defaultScale;

    private void Start()
    {

        _hoverScale = gameObject.transform.localScale * _relativeScaleOnHover;
        _defaultScale = gameObject.transform.localScale;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        gameObject?.transform.DOScale(_hoverScale, _timeToScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        gameObject?.transform.DOScale(_defaultScale, _timeToScale);
    }
}
