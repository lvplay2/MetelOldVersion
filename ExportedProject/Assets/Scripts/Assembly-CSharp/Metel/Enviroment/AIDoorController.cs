using Metel.Bot;
using UnityEngine;

namespace Metel.Enviroment
{
	public class AIDoorController : MonoBehaviour
	{
		public const float DistanseReaction = 2f;

		public Vector3 closedRotation;

		public Vector3 openedRotation;

		public static float SpeedRotate = 120f;

		private BotLogic _controller;

		[SerializeField]
		private BarController _bar;

		[SerializeField]
		private float timeBlocked = 50f;

		private bool tmp;

		private AudioSource _as;

		[SerializeField]
		private AudioClip doorOpen;

		[SerializeField]
		private AudioClip doorClose;

		private bool _tmp;

		private bool _tmpSounds;

		private bool breaked;

		private float _distanse
		{
			get
			{
				return Vector3.Distance(base.transform.position, _controller.transform.position);
			}
		}

		private bool ChangedState
		{
			get
			{
				return Vector3.Distance(base.transform.localRotation.eulerAngles, targetRotation) > 0.1f;
			}
		}

		private Vector3 targetRotation
		{
			get
			{
				return (!_tmp) ? closedRotation : openedRotation;
			}
		}

		public void ChangeDirection(bool value)
		{
			_tmp = value;
		}

		private void PlaySound(bool a)
		{
			if ((bool)_as)
			{
				_as.clip = ((!a) ? doorClose : doorOpen);
				_as.Play();
			}
		}

		private void Start()
		{
			_controller = Object.FindObjectOfType<BotLogic>();
			_as = GetComponent<AudioSource>();
		}

		public void Break()
		{
			tmp = true;
		}

		private void PlayKnock()
		{
			_bar.PlayKnockSound();
		}

		private void Update()
		{
			_tmpSounds = _tmp;
			if (ChangedState)
			{
				base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, Quaternion.Euler(targetRotation), SpeedRotate * Time.deltaTime);
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(targetRotation);
			}
			if (tmp && timeBlocked > 0f)
			{
				timeBlocked -= Time.deltaTime;
				PlayKnock();
			}
			if (_bar.IsActivated && timeBlocked < 0f)
			{
				_bar.Break();
				ChangeDirection(_distanse < 2f);
			}
			else if (_bar.IsActivated && timeBlocked > 0f && _distanse < 2f)
			{
				Break();
				breaked = true;
			}
			else if (!_bar.IsActivated)
			{
				ChangeDirection(_distanse < 2f);
			}
			if (!breaked && _tmp != _tmpSounds)
			{
				PlaySound(_distanse < 2f);
			}
		}
	}
}
