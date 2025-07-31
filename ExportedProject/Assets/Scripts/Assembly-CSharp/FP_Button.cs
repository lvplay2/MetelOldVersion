using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(EventTrigger))]
public class FP_Button : MonoBehaviour
{
	public Canvas myCanvas;

	public float defaultAlpha = 0.5f;

	public float activeAlpha = 1f;

	public bool Interactable = true;

	public bool Dynamic;

	public bool isToggleOnStart;

	private bool isPressed;

	private bool toggle;

	private bool clicked;

	private bool released;

	private CanvasGroup canvasGroup;

	private EventTrigger eventTrigger;

	private Vector2 touchInput;

	private Vector2 prevDelta;

	private Vector2 dragInput;

	private Vector2 defaultPos;

	private Vector2 targetPos;

	private RectTransform rect;

	private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		canvasGroup.alpha = defaultAlpha;
		rect = GetComponent<RectTransform>();
		defaultPos = rect.anchoredPosition;
		toggle = isToggleOnStart;
		SetupListeners();
	}

	private void Update()
	{
		touchInput = (dragInput - prevDelta) / Time.deltaTime;
		prevDelta = dragInput;
		if (isPressed)
		{
			if (Interactable)
			{
				canvasGroup.alpha = activeAlpha;
			}
		}
		else
		{
			canvasGroup.alpha = defaultAlpha;
		}
		if (Dynamic)
		{
			if (isPressed)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, dragInput, myCanvas.worldCamera, out targetPos);
				rect.position = myCanvas.transform.TransformPoint(targetPos);
			}
			else
			{
				rect.anchoredPosition = defaultPos;
			}
		}
	}

	private void SetupListeners()
	{
		eventTrigger = base.gameObject.GetComponent<EventTrigger>();
		EventTrigger.TriggerEvent triggerEvent = new EventTrigger.TriggerEvent();
		triggerEvent.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			data.Use();
			isPressed = true;
			toggle = !toggle;
			prevDelta = (dragInput = pointerEventData.position);
			StartCoroutine("WasClicked");
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent,
			eventID = EventTriggerType.PointerDown
		});
		EventTrigger.TriggerEvent triggerEvent2 = new EventTrigger.TriggerEvent();
		triggerEvent2.AddListener(delegate(BaseEventData data)
		{
			PointerEventData pointerEventData = (PointerEventData)data;
			data.Use();
			dragInput = pointerEventData.position;
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent2,
			eventID = EventTriggerType.Drag
		});
		EventTrigger.TriggerEvent triggerEvent3 = new EventTrigger.TriggerEvent();
		triggerEvent3.AddListener(delegate
		{
			touchInput = Vector2.zero;
			isPressed = false;
			StartCoroutine("WasReleased");
		});
		eventTrigger.triggers.Add(new EventTrigger.Entry
		{
			callback = triggerEvent3,
			eventID = EventTriggerType.PointerUp
		});
	}

	private IEnumerator WasClicked()
	{
		clicked = true;
		yield return null;
		clicked = false;
	}

	private IEnumerator WasReleased()
	{
		released = true;
		yield return null;
		released = false;
	}

	public void SetToggle(bool value)
	{
		toggle = value;
	}

	public Vector2 MoveInput()
	{
		return touchInput * Time.deltaTime;
	}

	public bool IsPressed()
	{
		return isPressed;
	}

	public bool OnPress()
	{
		return clicked;
	}

	public bool OnRelease()
	{
		return released;
	}

	public bool Toggle()
	{
		return toggle;
	}
}
