using UnityEngine;
using DG.Tweening;

public class DoTweenAnimation : MonoBehaviour
{
    protected Sequence _playingSequence;

    protected void TryKillAndCreateNewSequence()
    {
        TryKillSequence();

        _playingSequence = DOTween.Sequence();
    }

    protected void TryKillSequence()
    {
        if (_playingSequence == null) return;

        _playingSequence.Kill();
    }
}
