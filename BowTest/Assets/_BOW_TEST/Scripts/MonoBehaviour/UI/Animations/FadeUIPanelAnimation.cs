using UnityEngine;
using DG.Tweening;
using System;

public class FadeUIPanelAnimation : DoTweenAnimation, IUIPanelAnimation
{
    [SerializeField] private CanvasGroup _animatingGroup;
    [SerializeField] private float _animationsDuration;

    public void PlayHideAnimation(Action OnComplete = null)
    {
        TryKillAndCreateNewSequence();

        _playingSequence.Append(_animatingGroup.DOFade(0, _animationsDuration));
        if (OnComplete != null) _playingSequence.OnComplete(() => OnComplete());
    }

    public void PlayShowAnimation(Action OnComplete = null)
    {
        TryKillAndCreateNewSequence();
        if (_animatingGroup.alpha == 1) _animatingGroup.alpha = 0;

        _playingSequence.Append(_animatingGroup.DOFade(1, _animationsDuration));
        if (OnComplete != null) _playingSequence.OnComplete(() => OnComplete());
    }

    public void ResetState()
    {
        _animatingGroup.alpha = 1;
    }
}
