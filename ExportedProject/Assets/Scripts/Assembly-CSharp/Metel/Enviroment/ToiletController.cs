using Metel.Statistics;
using UnityEngine;

namespace Metel.Enviroment
{
	public class ToiletController : MonoBehaviour
	{
		public GameObject[] screws;

		public GameObject destroyAfter;

		private Animator _anims;

		private bool tmpStats;

		private AudioSource _as;

		private void Start()
		{
			_anims = GetComponent<Animator>();
			_as = GetComponent<AudioSource>();
		}

		public void Open()
		{
			if (!CanBeOpen())
			{
				return;
			}
			if ((bool)_anims)
			{
				_anims.SetTrigger("open");
			}
			if ((bool)destroyAfter)
			{
				Object.Destroy(destroyAfter);
			}
			if (!tmpStats)
			{
				AmplitudeStats.SendLog("Toilet opened");
				tmpStats = true;
				if ((bool)_as)
				{
					_as.Play();
				}
			}
		}

		private bool CanBeOpen()
		{
			for (byte b = 0; b < screws.Length; b++)
			{
				if (screws[b] != null)
				{
					return false;
				}
			}
			return true;
		}
	}
}
