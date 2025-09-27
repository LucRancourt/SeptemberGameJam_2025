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

    private Vector3 _dockedPosition, _dragPosition;
    private Vector2 _offset;
    private bool _IsOnTarget, _IsHeld;


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
        _offset = new Vector2(gameObject.transform.position.x - Mouse.current.position.ReadValue().x,
                                 gameObject.transform.position.y - Mouse.current.position.ReadValue().y);
    }

    public void OnDrag()
    {
        _dragPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        _dragPosition.z = _dockedPosition.z;
        gameObject.transform.position = _dragPosition;/// + _offset;
    }

    public void OnRelease()
    {
        if (!_IsHeld)
            return; 

        _IsHeld = false;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Vector2.zero, 100, _targetMask);

        if (hit && hit.collider.gameObject.GetComponent<DropTarget>() != null)
        {
            hit.collider.gameObject.GetComponent<DropTarget>().SetHeldWord(this);
            gameObject.transform.DOMove(hit.collider.gameObject.GetComponent<DropTarget>().DropPosition, _timeToDock / 3f);
        }
        else
        {
            gameObject.transform.DOMove(_dockedPosition, _timeToDock);
        }
    }

    #region Getters/Setters

    public bool IsOnTarget => _IsOnTarget;
    public string GetWord() { return _word; }
    public void SetDockedPosition(Vector3 position) { _dockedPosition = position; }

    #endregion
}

/*
    #region Implemented from Interfaces (Drag and Drop) 

    public void OnDrag(PointerEventData eventData)
    {
        //When the word is clicked on
        Debug.Log("Bubble has been clicked on");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Whenever the held word moves

        gameObject.transform.position += (Vector3)eventData.delta;
        Debug.Log("Bubble is being moved");
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Bubble has been released");
        //Whenever the word is released

        //Check for valid target

        //if valid, move to target position

        //else, return to docked position
        gameObject.transform.DOMove(_dockedPosition, _timeToDock);
    }
    #endregion
*/
