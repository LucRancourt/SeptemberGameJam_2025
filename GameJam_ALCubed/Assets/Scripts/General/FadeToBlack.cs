using System;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : Singleton<FadeToBlack>
{
    public event Action FadeComplete;

    private Image panel;
    private float _fadeValue;
    private bool _timeToFade;

    [SerializeField, Range(0, 5.0f)] private float dividerToSlowFade = 3.0f;

    private void Start()
    {
        panel = GetComponentInChildren<Image>();

        _fadeValue = 0.0f;
        SetOpacity(_fadeValue);
        _timeToFade = false;
    }

    public void Fade()
    {
        _timeToFade = true;
    }

    private void SetOpacity(float alphaValue)
    {
        Color tempColor = panel.color;

        tempColor.a = Mathf.Clamp01(alphaValue);

        panel.color = tempColor;
    }

    private void Update()
    {
        if (_timeToFade)
        {
            _fadeValue += Time.deltaTime / dividerToSlowFade;
            SetOpacity(_fadeValue);

            if (_fadeValue >= 1.5f)
            {
                _timeToFade = false;
                FadeComplete?.Invoke();
            }
        }
    }
}