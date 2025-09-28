using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class StickAnimationController : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayStartAnimation(int panelIndex)
    {
        string stateName = panelIndex == 0 ? "Start" : $"Start{panelIndex}";
        Debug.Log($"[StickAnim] >>> PlayStartAnimation: {stateName} (layer {panelIndex})");
        _animator.Play(stateName, panelIndex);
    }

    public void PlayIdleAnimation(int panelIndex)
    {
        string stateName = panelIndex == 0 ? "Idle" : $"Idle{panelIndex}";
        Debug.Log($"[StickAnim] >>> PlayIdleAnimation: {stateName} (layer {panelIndex})");
        _animator.Play(stateName, panelIndex);
    }

    public void PlayEndAnimation(int panelIndex)
    {
        string stateName = panelIndex == 0 ? "End" : $"End{panelIndex}";
        Debug.Log($"[StickAnim] >>> PlayEndAnimation: {stateName} (layer {panelIndex})");
        _animator.Play(stateName, panelIndex);
    }

    public void PlayFailAnimation(int panelIndex)
    {
        string stateName = panelIndex == 0 ? "Fail" : $"Fail{panelIndex}";
        Debug.Log($"[StickAnim] >>> PlayFailAnimation: {stateName} (layer {panelIndex})");
        _animator.Play(stateName, panelIndex);
    }

    // --- Animation Events ---
    public void OnStartAnimationFinished(int panelIndex)
    {
        Debug.Log($"[StickAnim] Panel {panelIndex} Start finished → Idle");
        PlayIdleAnimation(panelIndex);
    }

    public void OnIdleAnimationFinished(int panelIndex)
    {
        Debug.Log($"[StickAnim] Idle finished → checking panel {panelIndex}");
        bool success = CameraController.Instance.CheckCurrentPanel();

        if (success)
        {
            Debug.Log($"[StickAnim] Panel {panelIndex} success → play End");
            PlayEndAnimation(panelIndex);
        }
        else
        {
            Debug.Log($"[StickAnim] Panel {panelIndex} failed → play Fail");

            _animator.applyRootMotion = false;
            CameraController.Instance.TeleportToFailPosition(panelIndex);
            StartCoroutine(ReenableRootMotion());

            PlayFailAnimation(panelIndex);
        }
    }

    public void OnEndAnimationFinished(int panelIndex)
    {
        if (panelIndex == CameraController.Instance.PanelsCount - 2)
        {
            Debug.Log($"[StickAnim] Panel {panelIndex} End finished → jump to last panel");

            int lastPanel = CameraController.Instance.PanelsCount - 1;

            _animator.applyRootMotion = false;

            CameraController.Instance.ResetTransitionFlag();
            CameraController.Instance.ZoomToPanel(lastPanel);

            StartCoroutine(ReenableRootMotion());

            bool success = CameraController.Instance.CheckCurrentPanel();
            if (success)
            {
                Debug.Log($"[StickAnim] Last panel {lastPanel} success → End");
                PlayEndAnimation(lastPanel);
            }
            else
            {
                Debug.Log($"[StickAnim] Last panel {lastPanel} fail → Fail");
                PlayFailAnimation(lastPanel);
            }
        }
        else
        {
            Debug.Log($"[StickAnim] End finished on {panelIndex} → go to next panel");

            _animator.applyRootMotion = false;

            CameraController.Instance.ResetTransitionFlag();
            CameraController.Instance.ZoomToNextPanel();

            StartCoroutine(ReenableRootMotion());
        }
    }
    
    public void OnFailAnimationFinished(int panelIndex)
    {
        Debug.Log($"[StickAnim] Fail finished → reload scene");
        CameraController.Instance.TriggerSceneReload();
    }
    
    private IEnumerator ReenableRootMotion()
    {
        yield return null;
        _animator.applyRootMotion = true;
    }
}
