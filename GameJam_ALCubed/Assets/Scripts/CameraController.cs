using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class PanelData
{
    public GameObject panel;
    public float waitTime;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private PanelData[] panels;
    [SerializeField] private float transitionSpeed = 2f;
    [SerializeField] private float failDelay = 1.5f;
    //[SerializeField] private DragDropScript dragDropManager;


    private int _currentPanelIndex = -1;
    private Vector3 _targetPosition;
    private float _targetSize;

    private void Start()
    {
        ZoomOutToAllPanels();
        StartCoroutine(TestSequence());
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
        
        /*if (dragDropManager.playerPressedPlay)
        {
            dragDropManager.playerPressedPlay = false;
            ZoomToPanel(0);
        }*/
    }

    private void ZoomOutToAllPanels()
    {
        _targetPosition = new Vector3(0, 0, -10f);
        _targetSize = 10f;
        _currentPanelIndex = -1;
    }

    private void ZoomToPanel(int index)
    {
        if (index < 0 || index >= panels.Length) return;

        _currentPanelIndex = index;

        BoxCollider2D col = panels[index].panel.GetComponent<BoxCollider2D>();
        if (col != null)
        {
            Bounds b = col.bounds;
            _targetPosition = new Vector3(b.center.x, b.center.y, -10f);

            float sizeY = b.size.y / 2f;
            float sizeX = b.size.x / 2f / cam.aspect;
            _targetSize = Mathf.Max(sizeY, sizeX);
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void OnFail()
    {
        Invoke(nameof(ReloadScene), failDelay);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator TestSequence()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 0; i < panels.Length; i++)
        {
            ZoomToPanel(i);
            yield return new WaitForSeconds(panels[i].waitTime);
        }

        ZoomOutToAllPanels();
    }
}
