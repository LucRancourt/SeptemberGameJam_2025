using UnityEngine;
using UnityEngine.UI;

public class PanelHandler : MonoBehaviour
{
    [SerializeField] private DropTarget[] dragTargets;
    [SerializeField] private Button goButton;
    [SerializeField] private CameraController cameraController;

    private void Start()
    {
        goButton.interactable = false;
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
            return;
        }

        Debug.Log("[PanelHandler] GO pressed → checking current panel");
        cameraController.CheckCurrentPanel();
    }
}