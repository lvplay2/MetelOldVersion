using System.Collections;
using Metel.Bot;
using Metel.Player;
using UnityEngine;

namespace Metel.Enviroment
{
	public class LatticeController : MonoBehaviour
	{
		[SerializeField]
		private Transform doorTransform;

		private Switcher220V _switcher;

		private Animator _anims;

		private byte currentState;

		public byte[] States = new byte[3] { 0, 15, 9 };

		public bool IsUnlocked;

		public Vector3 closedRotation;

		public Vector3 openedRotation;

		public static float SpeedRotate = 120f;

		private AudioSource _as;

		public AudioClip openSound;

		public AudioClip closeSound;

		public AudioClip[] unlockSound;

		private BotLogic _bot;

		public bool isOpened { get; private set; }

		public bool HavePlayer { get; internal set; }

		public byte IDNeedItem
		{
			get
			{
				return States[currentState];
			}
		}

		private Vector3 targetRotation
		{
			get
			{
				return (!isOpened) ? closedRotation : openedRotation;
			}
		}

		private void Start()
		{
			_as = GetComponent<AudioSource>();
			_anims = GetComponent<Animator>();
			_switcher = Object.FindObjectOfType<Switcher220V>();
			_bot = Object.FindObjectOfType<BotLogic>();
			SetActiveEffect(false);
		}

		private void Update()
		{
			if (_bot.timeWait < 1f && isOpened)
			{
				ChangeState(false);
			}
			if (currentState < States.Length && States[currentState] == 0)
			{
				IsUnlocked = true;
			}
			if (Vector3.Distance(base.transform.localRotation.eulerAngles, targetRotation) > 0.1f)
			{
				doorTransform.localRotation = Quaternion.RotateTowards(doorTransform.localRotation, Quaternion.Euler(targetRotation), SpeedRotate * Time.deltaTime);
			}
			else
			{
				doorTransform.localRotation = Quaternion.Euler(targetRotation);
			}
			if (HavePlayer)
			{
				VoltageDead();
			}
		}

		private void PlaySound(bool unlock = false)
		{
			if ((bool)_as && !_as.isPlaying)
			{
				_as.clip = (unlock ? unlockSound[(currentState >= unlockSound.Length) ? (unlockSound.Length - 1) : currentState] : ((!isOpened) ? closeSound : openSound));
				_as.Play();
			}
		}

		public void SetActiveEffect(bool value)
		{
		}

		public void VoltageDead()
		{
			if ((bool)_switcher && _switcher.IsActivated)
			{
				StartCoroutine(DeadByVolt());
			}
		}

		private IEnumerator DeadByVolt()
		{
			PlayerHealth player = Object.FindObjectOfType<PlayerHealth>();
			player.Damage();
			yield return new WaitForSeconds(1f);
			player.Kill();
		}

		public void ChangeState()
		{
			if (IsUnlocked)
			{
				isOpened = !isOpened;
				PlaySound();
			}
		}

		public void ChangeState(bool value)
		{
			if (IsUnlocked)
			{
				if (isOpened != value)
				{
					PlaySound();
				}
				isOpened = value;
			}
		}

		public void Unlock()
		{
			PlaySound(true);
			VoltageDead();
			IsUnlocked = true;
		}

		public void NextState()
		{
			currentState++;
			if ((bool)_anims)
			{
				_anims.SetTrigger("reset");
			}
			IsUnlocked = false;
		}
	}
}
