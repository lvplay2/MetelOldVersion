using UnityEngine;

namespace Metel.Enviroment
{
	public class BoxController : MonoBehaviour
	{
		public Vector3 closedPosition;

		public Vector3 openedPosition;

		public float timeMove = 2f;

		private AudioSource _as;

		public AudioClip openSound;

		public AudioClip closeSound;

		public bool IsOpened { get; private set; }

		public float ToTargetPoint
		{
			get
			{
				return Vector3.Distance(base.transform.localPosition, targetPoint);
			}
		}

		private Vector3 targetPoint
		{
			get
			{
				return (!IsOpened) ? closedPosition : openedPosition;
			}
		}

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (ToTargetPoint < 0.01f)
			{
				base.transform.localPosition = targetPoint;
			}
			else
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, targetPoint, timeMove * Time.deltaTime);
			}
		}

		public void ChangeDirection()
		{
			if (!(ToTargetPoint > 0.01f))
			{
				IsOpened = !IsOpened;
				PlaySound();
			}
		}

		public void ChangeDirection(bool value)
		{
			if (!(ToTargetPoint > 0.01f))
			{
				IsOpened = value;
				PlaySound();
			}
		}

		private void PlaySound()
		{
			if ((bool)_as && !_as.isPlaying)
			{
				_as.clip = ((!IsOpened) ? closeSound : openSound);
				_as.Play();
			}
		}
	}
}
