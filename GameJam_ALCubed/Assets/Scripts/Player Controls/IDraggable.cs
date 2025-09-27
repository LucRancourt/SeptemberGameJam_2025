using UnityEngine;

//Instead of IDragHandler, since that can only be implemented for UI elements
public interface IDraggable
{
    public void OnClick();
    public void OnDrag();
    public void OnRelease();
}
