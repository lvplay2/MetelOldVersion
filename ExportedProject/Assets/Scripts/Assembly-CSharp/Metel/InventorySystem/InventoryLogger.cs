using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.InventorySystem
{
	public class InventoryLogger : MonoBehaviour
	{
		public Text _logger;

		public float messageRate = 2f;

		private void Start()
		{
			_logger = GameObject.Find("InvLogger").GetComponent<Text>();
			_logger.text = string.Empty;
		}

		public void ShowMessage(string message)
		{
			if (_logger == null)
			{
				Debug.LogError("Not finded 'InvLogger' game object");
			}
			else
			{
				StartCoroutine(logMessage(message, messageRate));
			}
		}

		private IEnumerator logMessage(string msg, float time)
		{
			_logger.text = msg;
			yield return new WaitForSeconds(time);
			_logger.text = string.Empty;
		}
	}
}
