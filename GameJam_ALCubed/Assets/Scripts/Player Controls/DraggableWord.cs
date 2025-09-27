using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/*
    The onomatopoeia word.
    Draggable from the docked position to any DragTarget
*/
public class DraggableWord : MonoBehaviour, IDraggable
{
    [SerializeField] string _word;
    [SerializeField] float _timeToDock = 0.7f;     //Time is takes to move back to the original docked position

    private Vector3 _dockedPosition;
    private Vector2 _offset;
    private bool _IsOnTarget, _IsHeld;

    private void Start()
    {
        _dockedPosition = gameObject.transform.position;
        InputManager.Instance.OnMouseRelease += OnRelease;
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
        gameObject.transform.position = Mouse.current.position.ReadValue() + _offset;;
    }

    public void OnRelease()
    {
        gameObject.transform.DOMove(_dockedPosition, _timeToDock);
        _IsHeld = false;
    }

    #region Getters/Setters

    public bool IsOnTarget => _IsOnTarget;

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
