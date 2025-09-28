using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelHandler : Singleton<PanelHandler>
{
    [SerializeField] private DropTarget[] dragTargets;
    [SerializeField] private Button goButton;
    [SerializeField] private CameraController cameraController;

    public event Action OnStartPressed;

    private void Start()
    {
        goButton.interactable = false;
        goButton.gameObject.SetActive(true);
        goButton.onClick.AddListener(OnGoPressed);
    }

    private void Update()
    {
        bool allFilled = true;

        foreach (DropTarget target in dragTargets)
        {
            if (!target.IsFilled())
            {
                allFilled = false;
                break;
            }
        }

        goButton.interactable = allFilled;
    }

    private void OnGoPressed()
    {
        if (cameraController.CurrentPanelIndex == -1)
        {
            cameraController.ZoomToPanel(0);
            OnStartPressed.Invoke();
            goButton.gameObject.SetActive(false);// = false;
            cameraController.CheckCurrentPanel();
            InputManager.Instance.DisableDragAndDrop();
            return;
        }

    }
}