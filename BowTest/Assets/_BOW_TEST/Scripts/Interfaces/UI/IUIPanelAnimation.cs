using System;

public interface IUIPanelAnimation
{
    void PlayShowAnimation(Action OnComplete = null);
    void PlayHideAnimation(Action OnComplete = null);
    void ResetState();
}
