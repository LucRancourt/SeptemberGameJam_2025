using System.Collections;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent (typeof (SpriteRenderer))]
public class DropTarget : MonoBehaviour
{
    [SerializeField] private string _correctWord;
    [SerializeField] private GameObject _successPanel;

    [Header("Expansion Sequence")]
    [SerializeField] private float _timeToExpand = 0.6f;
    [SerializeField] private float _relativeSize = 1.5f;
    private Vector3 _expandedScale;

    private DraggableWord _heldWord;
    private Vector3 _dropPosition;
    private Sequence _expansionSequence;

    private void Start()
    {
        _dropPosition = transform.position;
        _dropPosition.z -= 1;

        _expansionSequence = DOTween.Sequence();
        _expansionSequence.Pause();
        _expansionSequence.SetAutoKill(false);
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

    public IEnumerator ExpandAndSwapPanel()
    {
        if (!IsHoldingWord())
        {
            yield return 0;

        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
           
            _heldWord.ShowWord();       //ToDO: set HideWord and spriterenderer disable to start button click event
            _heldWord.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            _heldWord?.transform.DOScale(_expandedScale, _timeToExpand);
            yield return new WaitForSeconds(_timeToExpand + 0.3f);          //magic number to get rid of (the amount of time spent expanded)

            if (_successPanel != null)
                _successPanel?.SetActive(true);      //changed panel becomes visible       

            _heldWord?.transform.DOScale(-_expandedScale, _timeToExpand).SetRelative(true);
        }
    }


    public IEnumerator TestExpansion(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(ExpandAndSwapPanel());
    }

    #region Getter/Setter
    public Vector3 DropPosition => _dropPosition;

    public void SetHeldWord(DraggableWord word)
    {
        if (word == null) return;

        _heldWord = word;
        _expandedScale = _heldWord.transform.localScale * _relativeSize;
    }
    public void ClearHeldWord() { _heldWord = null; }

    public DraggableWord GetHeldWord() { return this._heldWord; }
    public bool IsHoldingWord() { return (_heldWord != null); }
    #endregion
}