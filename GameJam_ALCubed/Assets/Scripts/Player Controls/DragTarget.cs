using UnityEngine;

public class DropTarget : MonoBehaviour
{
    [SerializeField] string _correctWord;


    private DraggableWord _heldWord;
    private Vector3 _dropPosition;

    private void Start()
    {
        _dropPosition = gameObject.transform.position;
        _dropPosition.z -= 1;
    }

    public bool IsHeldWordValid()
    {
        if (_heldWord == null && _correctWord != "")    //just in case some panels dont actually have required words?
            return false;

        return (_heldWord.GetWord().ToUpper().Equals(_correctWord.ToUpper()));
    }

    #region Getter/Setter
    public Vector3 DropPosition => _dropPosition;
    public void SetHeldWord(DraggableWord word) { this._heldWord = word; }
    public bool IsHoldingWord() { return (_heldWord != null); }
    #endregion
}
