using UnityEngine;
using UnityEngine.EventSystems;

namespace Metel.UI
{
	[RequireComponent(typeof(EventTrigger))]
	public class MouseInputUIBlocker : MonoBehaviour
	{
		public static bool BlockedByUI;

		private EventTrigger eventTrigger;

		private void Start()
		{
			eventTrigger = GetComponent<EventTrigger>();
			if (eventTrigger == null)
			{
				eventTrigger = base.gameObject.AddComponent<EventTrigger>();
			}
			if (eventTrigger != null)
			{
				EventTrigger.Entry entry = new EventTrigger.Entry();
				entry.eventID = EventTriggerType.PointerEnter;
				entry.callback.AddListener(delegate
				{
					EnterUI();
				});
				eventTrigger.triggers.Add(entry);
				EventTrigger.Entry entry2 = new EventTrigger.Entry();
				entry2.eventID = EventTriggerType.PointerExit;
				entry2.callback.AddListener(delegate
				{
					ExitUI();
				});
				eventTrigger.triggers.Add(entry2);
			}
		}

		public void EnterUI()
		{
			BlockedByUI = true;
		}

		public void ExitUI()
		{
			BlockedByUI = false;
		}
	}
}
