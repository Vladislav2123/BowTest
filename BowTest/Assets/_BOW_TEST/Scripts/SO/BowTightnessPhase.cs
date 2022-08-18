using UnityEngine;

[CreateAssetMenu]
public class BowTightnessPhase : ScriptableObject
{
    [SerializeField] private Interval _timePeriod;
    [SerializeField] private float _staminaPrice;
    [SerializeField] [Range(0, 1)] private float _accuracy;
    [SerializeField] private float _damage;

    public Interval TimePeriod => _timePeriod;
    public float StaminaPrice => _staminaPrice;
    public float Accuracy => _accuracy;
    public float Damage => _damage;
}
