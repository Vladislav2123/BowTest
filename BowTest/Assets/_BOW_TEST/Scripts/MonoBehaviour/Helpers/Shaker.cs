using UnityEngine;
using DG.Tweening;

public class Shaker : MonoBehaviour
{
    [SerializeField] private Transform _shakingTransform;

    [Header("=== POSITION SHAKE ===")]
    [SerializeField] private bool _shakePosition;
    [SerializeField] private float _positionShakeDuration;
    [SerializeField] private int _positionShakeVibrato = 10;
    [SerializeField] private Vector3 _positionShakeStrength;

    [Header("=== ROTATION SHAKE ===")]
    [SerializeField] private bool _shakeRotation;
    [SerializeField] private float _rotationShakeDuration;
    [SerializeField] private int _rotationShakeVibrato = 10;
    [SerializeField] private Vector3 _rotationShakeStrength;

    private void Awake()
    {
        if (_shakingTransform == null) _shakingTransform = transform;
    }

    public void Shake()
    {
        if (_shakePosition) _shakingTransform.DOShakePosition(_positionShakeDuration, _positionShakeStrength, _positionShakeVibrato);
        if (_shakeRotation) _shakingTransform.DOShakeRotation(_rotationShakeDuration, _rotationShakeStrength, _rotationShakeVibrato);
    }
}
