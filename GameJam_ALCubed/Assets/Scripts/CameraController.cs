using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class PanelData
{
    public GameObject panel;
    public float waitTime;
    public DropTarget[] dragTargets;
}

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private Camera cam;
    [SerializeField] private PanelData[] panels;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float failDelay = 1.5f;

    public int CurrentPanelIndex => _currentPanelIndex;

    private int _currentPanelIndex = -1;
    private Vector3 _targetPosition;
    private float _targetSize;

    private void Start()
    {
        ZoomOutToAllPanels();
        //DontDestroyOnLoad(gameObject);
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
        Debug.Log($"[CameraController] Zooming to panel {index}");

        BoxCollider2D col = panels[index].panel.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            _targetPosition = new Vector3(b.center.x, b.center.y, -10f);

            float sizeY = b.size.y / 2f;
            float sizeX = b.size.x / 2f / cam.aspect;
            _targetSize = Mathf.Max(sizeY, sizeX);
            
            StopAllCoroutines();
            StartCoroutine(CheckAfterDelay(index));
        }
    }
    
    private IEnumerator CheckAfterDelay(int panelIndex)
    {
        float wait = panels[panelIndex].waitTime;
        if (wait > 0)
            yield return new WaitForSeconds(wait);

        Debug.Log($"[CameraController] Wait finished → checking panel {panelIndex}");
        CheckCurrentPanel();
    }

    public void CheckCurrentPanel()
    {
        if (_currentPanelIndex < 0 || _currentPanelIndex >= panels.Length)
        {
            Debug.LogWarning("[CameraController] CheckCurrentPanel called with invalid index!");
            return;
        }

        Debug.Log($"[CameraController] Checking panel {_currentPanelIndex}");

        bool allCorrect = true;
        foreach (DropTarget target in panels[_currentPanelIndex].dragTargets)
        {
            if (!target.IsCorrect())
            {
                Debug.Log($"[CameraController] Target {target.name} is WRONG");
                allCorrect = false;
                break;
            }
            else
            {
                Debug.Log($"[CameraController] Target {target.name} is CORRECT");
            }
        }

        if (allCorrect)
        {
            Debug.Log("[CameraController] All correct → SUCCESS");
            OnSuccessfulGuess();
        }
        else
        {
            Debug.Log("[CameraController] Some wrong → FAIL");
            OnFail();
        }
    }

    public void OnSuccessfulGuess()
    {
        StartCoroutine(GoToNextPanelWithDelay());
    }

    private IEnumerator GoToNextPanelWithDelay()
    {
        int nextPanel = _currentPanelIndex + 1;

        if (nextPanel < panels.Length)
        {
            float wait = (_currentPanelIndex >= 0) ? panels[_currentPanelIndex].waitTime : 0f;

            if (wait > 0)
                yield return new WaitForSeconds(wait);

            ZoomToPanel(nextPanel);
        }
        else
        {
            float wait = (_currentPanelIndex >= 0) ? panels[_currentPanelIndex].waitTime : 0f;

            if (wait > 0)
                yield return new WaitForSeconds(wait);

            SceneManager.LoadScene("MainMenu");
        }
    }

    private void OnFail()
    {
        Invoke(nameof(ReloadScene), failDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
