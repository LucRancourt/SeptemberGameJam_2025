using System.Security.Cryptography;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
    The onomatopoeia word.
    Draggable from the docked position to any DragTarget
*/
public class DraggableWord : MonoBehaviour, IDraggable
{
    [SerializeField] string _word;
    [SerializeField] float _timeToDock = 0.7f;     //Time is takes to move back to the original docked position
    [SerializeField] LayerMask _targetMask;     //Time is takes to move back to the original docked position
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _dragDepth = -1f;

    private Vector3 _dockedPosition, _dragPosition;
    private Vector2 _offset;
    private bool _IsOnTarget, _IsHeld;

    private DropTarget _currentTarget;


    private void Start()
    {
        _dockedPosition = gameObject.transform.position;
        InputManager.Instance.OnMouseRelease += OnRelease;
        _text.text = _word;
    }

    private void Update()
    {
        if (_IsHeld)
            OnDrag();
    }

    public void OnClick()
    {
        _IsHeld = true;
    }

    public void OnDrag()
    {
        _dragPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _dragPosition.z = _dragDepth;
        gameObject.transform.position = _dragPosition;
    }

    public void OnRelease()
    {
        if (!_IsHeld)
            return;

        _IsHeld = false;

        RaycastHit2D hit = Physics2D.Raycast(
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()),
            Vector2.zero,
            100,
            _targetMask
        );

        if (hit && hit.collider.TryGetComponent(out DropTarget newTarget))
        {
            if (_currentTarget != null)
                _currentTarget.ClearHeldWord();

            _currentTarget = newTarget;
            _currentTarget.SetHeldWord(this);

            transform.DOMove(newTarget.DropPosition, _timeToDock / 3f);
        }
        else
        {
            if (_currentTarget != null)
            {
                _currentTarget.ClearHeldWord();
                _currentTarget = null;
            }

            transform.DOMove(_dockedPosition, _timeToDock);
        }
    }


    #region Getters/Setters

    public string GetWord() { return _word; }
    public void SetDockedPosition(Vector3 position) { _dockedPosition = position; }

    #endregion
}
