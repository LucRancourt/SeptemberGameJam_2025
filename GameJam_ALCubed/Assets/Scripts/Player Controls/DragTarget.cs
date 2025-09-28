using UnityEngine;

public class DropTarget : MonoBehaviour
{
    [SerializeField] private string _correctWord;

    private DraggableWord _heldWord;
    private Vector3 _dropPosition;

    private void Start()
    {
        _dropPosition = transform.position;
        _dropPosition.z -= 1;
    }

    public bool IsFilled()
    {
        return _heldWord != null;
    }

    public bool IsCorrect()
    {
        if (_heldWord == null) return false;
        return _heldWord.GetWord().ToUpper().Equals(_correctWord.ToUpper());
    }

    #region Getter/Setter
    public Vector3 DropPosition => _dropPosition;
    
    public void SetHeldWord(DraggableWord word) { _heldWord = word; }
    public void ClearHeldWord() { _heldWord = null; }

    public DraggableWord GetHeldWord() { return this._heldWord; }
    public bool IsHoldingWord() { return (_heldWord != null); }
    #endregion
}