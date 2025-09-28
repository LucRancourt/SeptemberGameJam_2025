using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class StickAnimationController : MonoBehaviour
{
    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();

        PlayEndAnimation(0);
    }

    public void PlayStartAnimation(int panelIndex)
    {
        try
        {
            _animator.Play("Start", panelIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void PlayIdleAnimation(int panelIndex)
    {
        try
        {
            _animator.Play("Idle", panelIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void PlayEndAnimation(int panelIndex)
    {
        try
        {
            _animator.Play("End", panelIndex);
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }

    public void OnStartAnimationFinished()
    {
        Debug.Log("Animation Finished!");
    }

    public void OnEndAnimationFinished()
    {
        Debug.Log("Animation Finished!");

    }

}
