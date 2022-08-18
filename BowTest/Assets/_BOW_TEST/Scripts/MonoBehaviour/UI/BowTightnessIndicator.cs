using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


public class BowTightnessIndicator : UIPanel
{
    [Header("=== SLIDER ===")]
    [SerializeField] private Image _slider;
    [SerializeField] private Gradient _sliderGradient;
    [SerializeField] private GameObject _phaseBoundPreab;

    private RectTransform _contentRect;
    private List<BowTightnessPhase> _bowTightnessPhases;
    private BowTightnessPhase _maxBowTightnessPhase;

    private RectTransform ContentRect
    {
        get
        {
            if(_contentRect == null) _contentRect = _content.GetComponent<RectTransform>();
            return _contentRect;
        }
    }
    private float ContentHeight => ContentRect.rect.height;

    public void Init(List<BowTightnessPhase> phases)
    {
        _bowTightnessPhases = phases;
        _maxBowTightnessPhase = _bowTightnessPhases.OrderBy(phase => phase.TimePeriod.MaxValue).Last();

        for(int i = 0; i < _bowTightnessPhases.Count - 1; i++)
        {
            CreatePhaseBound(_bowTightnessPhases[i]);
        }
    }

    public void SetSliderByTightTimer(float time)
    {
        float t = time / _maxBowTightnessPhase.TimePeriod.MaxValue;
        _slider.fillAmount = t;
        _slider.color = _sliderGradient.Evaluate(t);
    }

    private void CreatePhaseBound(BowTightnessPhase phase)
    {
        GameObject newBoundPrefab = Instantiate(_phaseBoundPreab, _content.transform);
        float t = phase.TimePeriod.MaxValue / _maxBowTightnessPhase.TimePeriod.MaxValue;
        newBoundPrefab.transform.localPosition = GetLocalPointInsideByTime(t);
    }

    private Vector2 GetLocalPointInsideByTime(float t)
    {
        float pointHeight = ContentHeight * t;
        return new Vector2(0, (-ContentHeight / 2) + pointHeight);
    }
}
