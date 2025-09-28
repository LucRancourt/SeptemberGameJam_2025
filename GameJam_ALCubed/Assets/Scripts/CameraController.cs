using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PanelData
{
    public GameObject panel;
    public DropTarget[] dragTargets;
    public Transform startPoint;
    public float failYOffset;
}

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera cam;
    [SerializeField] private PanelData[] panels;
    [SerializeField] private Transform stickman;

    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float failDelay = 1.5f;

    public int CurrentPanelIndex => _currentPanelIndex;
    public int PanelsCount => panels.Length;

    private int _currentPanelIndex = -1;
    private Vector3 _targetPosition;
    private float _targetSize;
    private bool _isTransitioning = false;

    private void Start()
    {
        ZoomOutToAllPanels();
    }

    private void Update()
    {
        cam.transform.position = Vector3.Lerp(
            cam.transform.position,
            _targetPosition,
            Time.deltaTime * transitionSpeed
        );

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            _targetSize,
            Time.deltaTime * transitionSpeed
        );
    }

    private void ZoomOutToAllPanels()
    {
        _targetPosition = new Vector3(0, 0, -10f);
        _targetSize = 10f;
        _currentPanelIndex = -1;
    }

    public void ZoomToPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        _currentPanelIndex = index;
        Debug.Log($"[CameraController] ZoomToPanel({index})");

        BoxCollider2D col = panels[index].panel.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            _targetPosition = new Vector3(b.center.x, b.center.y, -10f);

            float sizeY = b.size.y / 2f;
            float sizeX = b.size.x / 2f / cam.aspect;
            _targetSize = Mathf.Max(sizeY, sizeX);
        }

        if (stickman != null && panels[index].startPoint != null)
        {
            stickman.position = panels[index].startPoint.position;
        }

        StickAnimationController stickAnim = FindFirstObjectByType<StickAnimationController>();
        if (stickAnim != null)
        {
            stickAnim.PlayStartAnimation(index);
        }
    }

    public void TeleportToFailPosition(int index)
    {
       stickman.transform.Translate(0f, panels[index].failYOffset, 0f);
    }

    public bool CheckCurrentPanel()
    {
        if (_currentPanelIndex < 0 || _currentPanelIndex >= panels.Length)
            return false;

        foreach (DropTarget target in panels[_currentPanelIndex].dragTargets)
        {
            if (!target.IsCorrect())
                return false;
        }

        return true;
    }

    public void ZoomToNextPanel()
    {
        if (_isTransitioning) return;
        _isTransitioning = true;

        int nextPanel = _currentPanelIndex + 1;

        if (nextPanel < panels.Length)
        {
            ZoomToPanel(nextPanel);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }

        _isTransitioning = false;
    }

    public void TriggerSceneReload()
    {
        Invoke(nameof(ReloadScene), failDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetTransitionFlag()
    {
        _isTransitioning = false;
    }
}
