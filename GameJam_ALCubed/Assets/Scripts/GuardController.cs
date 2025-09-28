using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardController : MonoBehaviour
{
    private Animator _animator;

    [SerializeField] private string idleAnim = "Idle";
    [SerializeField] private string failAnim = "ReactFail";
    [SerializeField] private string successAnim = "ReactSuccess";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _animator.Play(idleAnim);
    }

    public void PlayFail()
    {
        Debug.Log("Guard Fail animasyonu tetiklendi");
        _animator.Play("ReactFail", 0, 0);
    }

    public void PlaySuccess()
    {
        Debug.Log("Guard Success animasyonu tetiklendi");
        _animator.Play("ReactSuccess", 0, 0);
    }
}