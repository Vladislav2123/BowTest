using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    [Inject] private InputHandler _inputHandler;

    public void OnPointerDown(PointerEventData eventData)
    {
        _inputHandler.IsPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _inputHandler.IsPressed = false;
    }
}
