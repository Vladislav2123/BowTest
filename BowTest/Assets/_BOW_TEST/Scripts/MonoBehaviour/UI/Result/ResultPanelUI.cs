using UnityEngine;
using DG.Tweening;

public class ResultPanelUI : UIPanel
{
    [Header("=== TITLE ===")]
    [SerializeField] private Transform _titleTransform;
    [SerializeField] private float _titleAnimationDuration;
    [SerializeField] private Ease _titleAnimationEase;

    public override bool TryShow()
    {
        if (base.TryShow() == false) return false;

        PlayTitleAnimation();
        return true;
    }

    private void PlayTitleAnimation()
    {
        _titleTransform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        _titleTransform.DOScale(Vector3.one, _titleAnimationDuration).SetEase(_titleAnimationEase);
    }
}
