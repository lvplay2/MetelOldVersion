using System.Collections.Generic;
using UnityEngine;

namespace Metel.Statistics
{
	public class AmplitudeStats : MonoBehaviour
	{
		public bool IsDebug;

		private void Awake()
		{
			if (!IsDebug)
			{
				Amplitude instance = Amplitude.Instance;
				instance.logging = true;
				instance.init("fb9a747b6765981a70e5a48d9d109e1d");
			}
		}

		public static void SendLog(string log)
		{
			if (Amplitude.Instance == null)
			{
				Debug.LogError("Amplitude is not initialized");
			}
			else
			{
				Amplitude.Instance.logEvent(log);
			}
		}

		public static void SendLog(string log, Dictionary<string, object> d)
		{
			if (Amplitude.Instance == null)
			{
				Debug.LogError("Amplitude is not initialized");
			}
			else
			{
				Amplitude.Instance.logEvent(log, d);
			}
		}
	}
}
