using System.Collections.Generic;
using Metel.Player;
using UnityEngine;

namespace Metel.Statistics
{
	public class TimeInPlay : MonoBehaviour
	{
		public float timeUpdate = 15f;

		private float currentUpdate = 15f;

		private float currentTimeInGame;

		public const string SaveTag = "timeInGame";

		public const string SaveTagCurrentGame = "currentGameTime";

		private PlayerController _pc;

		private float currentGame;

		private void Start()
		{
			_pc = Object.FindObjectOfType<PlayerController>();
			Load();
		}

		private void Update()
		{
			if (_pc.IsGame)
			{
				currentGame += Time.deltaTime;
			}
			currentTimeInGame += Time.deltaTime;
			if (currentUpdate > 0f)
			{
				currentUpdate -= Time.deltaTime;
				return;
			}
			currentUpdate = timeUpdate;
			Send();
			Save();
		}

		public string GetCurrentGame()
		{
			if (currentGame > 0f)
			{
				int num = (int)currentGame / 60;
				int num2 = (int)currentGame - num * 60;
				return num + ":" + ((num2 >= 10) ? num2.ToString() : ("0" + num2));
			}
			return "0:00";
		}

		private void Send()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("Time", currentTimeInGame / 60f);
			dictionary.Add("DeviceID", Amplitude.Instance.getDeviceId());
			Dictionary<string, object> d = dictionary;
			AmplitudeStats.SendLog("TimeInGame", d);
		}

		public void ClearCurrentGameTime()
		{
			PlayerPrefs.DeleteKey("currentGameTime");
		}

		public void Save()
		{
			PlayerPrefs.SetFloat("timeInGame", currentTimeInGame);
			PlayerPrefs.SetFloat("currentGameTime", currentGame);
		}

		public void Load()
		{
			currentTimeInGame = PlayerPrefs.GetFloat("timeInGame", 0f);
			currentGame = PlayerPrefs.GetFloat("currentGameTime", 0f);
		}
	}
}
