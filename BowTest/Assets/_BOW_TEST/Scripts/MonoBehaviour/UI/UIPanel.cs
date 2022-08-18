using UnityEngine;

public class UIPanel : MonoBehaviour
{
    [SerializeField] protected GameObject _content;

    private IUIPanelAnimation _animationHandler;

    public GameObject Content => _content;
    public bool IsShowing => _content.activeInHierarchy;

    protected virtual void Awake()
    {
        _animationHandler = GetComponent<IUIPanelAnimation>();
    }

    public virtual bool TryShow()
    {
        if (IsShowing) return false;

        _content.SetActive(true);
        if (_animationHandler != null) _animationHandler.PlayShowAnimation();
        return true;
    }

    public virtual bool TryHide()
    {
        if (IsShowing == false) return false;

        if (_animationHandler != null) _animationHandler.PlayHideAnimation(() => _content.SetActive(false));
        else _content.SetActive(false);
        return true;
    }
} 
