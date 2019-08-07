using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputBtn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public bool pointerDown = false;

	public delegate void InputEventsDelegate();
	public InputEventsDelegate OnPointerDownEvents;
	public InputEventsDelegate OnPointerUpEvents;

	public void OnPointerDown(PointerEventData eventData)
	{
		pointerDown = true;
		OnPointerDownEvents.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pointerDown = false;
		OnPointerUpEvents.Invoke();
	}
}
