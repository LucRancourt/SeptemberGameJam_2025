using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _camTransform;
    private Vector3 _originalPos;

    public float shakeDuration = 0f;
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1.0f;

    void Awake()
    {
        Cursor.visible = false;
        _camTransform = GetComponent<Transform>();
    }

    void OnEnable()
    {
        _originalPos = _camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            _camTransform.localPosition = _originalPos + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            _camTransform.localPosition = _originalPos;
        }
    }

    public void ShakeCamera()
    {
        _originalPos = _camTransform.localPosition;
        shakeDuration = 0.2f;
    }
}