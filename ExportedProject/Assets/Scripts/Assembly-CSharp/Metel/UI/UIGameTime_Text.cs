using Metel.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.UI
{
	public class UIGameTime_Text : MonoBehaviour
	{
		[SerializeField]
		private TimeInPlay timeIn;

		private Text _text;

		private void Start()
		{
			if (!timeIn)
			{
				timeIn = Object.FindObjectOfType<TimeInPlay>();
			}
			_text = GetComponent<Text>();
		}

		private void Update()
		{
			if ((bool)_text)
			{
				_text.text = timeIn.GetCurrentGame();
			}
		}
	}
}
