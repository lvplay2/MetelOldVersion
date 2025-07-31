using Metel.Statistics;
using UnityEngine;

namespace Metel.Enviroment
{
	public class SinkController : MonoBehaviour
	{
		public ScrewController screw;

		public ChainController _chain;

		public bool status;

		private bool _shift;

		public Vector3 shiftPos;

		public float timeShift = 1f;

		private bool tmpStats;

		private AudioSource _as;

		public AudioClip shiftSound;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		private void PlaySound()
		{
			if ((bool)_as)
			{
				_as.clip = shiftSound;
				_as.Play();
			}
		}

		private void Update()
		{
			if (_shift)
			{
				base.transform.parent.localPosition = Vector3.MoveTowards(base.transform.parent.localPosition, shiftPos, timeShift * Time.deltaTime);
			}
		}

		public void Shift()
		{
			if (!screw)
			{
				_shift = true;
				if (!tmpStats)
				{
					PlaySound();
					AmplitudeStats.SendLog("Sink shift");
					tmpStats = true;
				}
			}
		}
	}
}
