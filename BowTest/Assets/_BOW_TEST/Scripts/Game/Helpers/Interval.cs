using UnityEngine;

[System.Serializable]
public class Interval
{
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;

    public float MinValue { get => _minValue; set => _minValue = value; }
    public float MaxValue { get => _maxValue; set => _maxValue = value; }
}
