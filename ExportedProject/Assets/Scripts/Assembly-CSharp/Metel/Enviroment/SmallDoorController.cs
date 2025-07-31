using UnityEngine;

namespace Metel.Enviroment
{
	public class SmallDoorController : MonoBehaviour
	{
		public const float SpeedRotate = 150f;

		public ChainController chain;

		private Vector3 startRot;

		public Vector3 endRot;

		private AudioSource _as;

		public AudioClip openSound;

		public AudioClip closeSound;

		private bool isUnlocked
		{
			get
			{
				return chain == null;
			}
		}

		public bool IsOpened { get; private set; }

		private Quaternion targetRotation
		{
			get
			{
				return Quaternion.Euler((!IsOpened) ? startRot : endRot);
			}
		}

		public void Open()
		{
			if (isUnlocked)
			{
				IsOpened = !IsOpened;
				PlaySound();
			}
		}

		public void SetOpened(bool value)
		{
			if (isUnlocked)
			{
				if (!IsOpened != value)
				{
					PlaySound();
				}
				IsOpened = value;
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

		private void Start()
		{
			startRot = base.transform.localRotation.eulerAngles;
			_as = GetComponent<AudioSource>();
		}

		private void Update()
		{
			base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, targetRotation, 150f * Time.deltaTime);
		}
	}
}
