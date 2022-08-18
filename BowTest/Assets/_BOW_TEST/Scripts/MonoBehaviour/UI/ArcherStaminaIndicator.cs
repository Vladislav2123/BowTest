using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class ArcherStaminaIndicator : MonoBehaviour
{
    [SerializeField] private Image _fillImage;
    [SerializeField] private Gradient _gradient;

    [Inject] private Archer _archer;

    private void Awake()
    {
        _archer.OnStaminaChangedEvent += RefreshIndicator;   
    }

    private void RefreshIndicator()
    {
        float normalizedValue = _archer.Stamina / _archer.MaxStamina;
        _fillImage.fillAmount = normalizedValue;
        _fillImage.color = _gradient.Evaluate(normalizedValue);
    }
}
