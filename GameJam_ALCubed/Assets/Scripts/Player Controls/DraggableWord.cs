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

    [Header("Render Ordering")]
    [SerializeField] float _dragDepth = -1f;
    [SerializeField] SpriteRenderer _spriteRenderer;
    [SerializeField] Canvas _canvas;

    [Header("SFX")]
    [SerializeField] private SFX _sfx;

    private Vector3 _dockedPosition, _dragPosition;
    private bool _IsHeld;

    private DropTarget _currentTarget;


    private void Start()
    {
        _dockedPosition = gameObject.transform.position;
        InputManager.Instance.OnMouseRelease += OnRelease;
        _text.text = _word;
        PanelHandler.Instance.OnStartPressed += HideWord;
    }

    private void Update()
    {
        if (_IsHeld)
            OnDrag();
    }

    #region Click and Drag

    public void OnClick()
    {
        _IsHeld = true;
        _spriteRenderer.sortingOrder += 2;
        _canvas.sortingOrder += 2;
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
        _spriteRenderer.sortingOrder -= 2;
        _canvas.sortingOrder -= 2;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 100, _targetMask);

        if (hit)
        {
            DropTarget target = hit.collider.gameObject.GetComponent<DropTarget>();

            if (target != null)
            {
                if (target != _currentTarget)        //If we are dopping over a new target
                    target.GetHeldWord()?.ReturnToDock();

                target.SetHeldWord(this);
                gameObject.transform.DOMove(target.DropPosition, _timeToDock / 3f);

                _currentTarget?.SetHeldWord(null);  //If on a target already, empty it
                _currentTarget = target;
            }
            else                                    //If we are dropping over a non-target
            {
                ReturnToDock();
            }

        }
        else
        {
            ReturnToDock();
        }
    }

    private void ReturnToDock()
    {
        gameObject.transform.DOMove(_dockedPosition, _timeToDock);
        if (_currentTarget != null)
        {
            _currentTarget.SetHeldWord(null);
            _currentTarget = null;
        }
    }
    #endregion

    public void HideWord()
    {
        _spriteRenderer.enabled = false;
        _canvas.enabled = false;
    }

    public void ShowWord()
    {
        _spriteRenderer.enabled = true;
        _canvas.enabled = true;


        AudioManager.Instance.PlaySound(_sfx);
    }

    #region Getters/Setters

    public string GetWord() { return _word; }
    public void SetDockedPosition(Vector3 position) { _dockedPosition = position; }

    #endregion
}