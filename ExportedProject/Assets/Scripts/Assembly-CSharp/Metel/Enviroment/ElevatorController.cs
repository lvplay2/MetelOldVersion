using System.Collections;
using Metel.Bot;
using UnityEngine;

namespace Metel.Enviroment
{
	public class ElevatorController : MonoBehaviour
	{
		public Vector3 bottomPosition;

		public Vector3 topPosition;

		public float speedMove = 2f;

		private Switcher220V _switcher220V;

		[SerializeField]
		private AudioSource _as;

		public AudioClip upMoveSound;

		public AudioClip downMoveSound;

		private bool isWait;

		public bool IsActivated { get; private set; }

		private Vector3 TargetPosition
		{
			get
			{
				return IsActivated ? topPosition : bottomPosition;
			}
		}

		private void Start()
		{
			_switcher220V = Object.FindObjectOfType<Switcher220V>();
		}

		public void ChangeDirection(bool value, bool force = false)
		{
			if (value != IsActivated)
			{
				if (value && value != IsActivated)
				{
					Object.FindObjectOfType<BotLogic>().Noise = true;
				}
				if (Vector3.Distance(base.transform.localPosition, TargetPosition) < 0.1f || force)
				{
					IsActivated = value;
					PlaySound(IsActivated);
				}
			}
		}

		public void PlaySound(bool up)
		{
			AudioClip audioClip = ((!up) ? downMoveSound : upMoveSound);
			if ((bool)_as && (_as.clip != audioClip || !_as.isPlaying))
			{
				_as.clip = audioClip;
				_as.PlayOneShot(audioClip);
			}
		}

		private void Update()
		{
			if (!_switcher220V.IsActivated && IsActivated)
			{
				ChangeDirection(false);
			}
			if (Vector3.Distance(base.transform.localPosition, TargetPosition) > 0.01f)
			{
				base.transform.localPosition = Vector3.MoveTowards(base.transform.localPosition, TargetPosition, speedMove * Time.deltaTime);
				return;
			}
			base.transform.localPosition = TargetPosition;
			_as.Stop();
		}

		private IEnumerator OffUPSound(float time)
		{
			isWait = true;
			_as.volume = Mathf.MoveTowards(_as.volume, 0f, time * Time.deltaTime);
			yield return new WaitForSeconds(time);
			isWait = false;
			_as.clip = downMoveSound;
			_as.Play();
		}
	}
}
