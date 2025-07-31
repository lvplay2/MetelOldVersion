using Metel.Player;
using Metel.Statistics;
using UnityEngine;

namespace Metel.Enviroment
{
	public class WinnerTrigger : MonoBehaviour
	{
		public GameObject ui;

		private bool tmpStats;

		private void Start()
		{
			ui.SetActive(false);
		}

		private void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == "Player")
			{
				ui.SetActive(true);
				if (!tmpStats)
				{
					AmplitudeStats.SendLog("Win");
					tmpStats = true;
					Object.FindObjectOfType<PlayerController>().Win();
				}
			}
		}
	}
}
