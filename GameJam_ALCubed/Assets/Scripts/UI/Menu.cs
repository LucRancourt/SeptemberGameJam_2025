using UnityEngine;
using UnityEngine.UI;

public class Menu<T> : Singleton<T> where T : MonoBehaviour
{
    // Variables 
    [SerializeField] private SFX clickSFX;

    private Button[] _menuButtons;


    // Functions
    protected override void Awake()
    {
        base.Awake();

        _menuButtons = GetComponentsInChildren<Button>(true);

        foreach (Button button in _menuButtons)
        {
            button.onClick.AddListener(PlayClickSFX);
        }
    }

    private void PlayClickSFX()
    {
        AudioManager.Instance.PlaySound(clickSFX);
    }
}