using UnityEngine;
using UnityEngine.AI;

namespace Metel.Enviroment
{
	public class BarController : MonoBehaviour
	{
		[SerializeField]
		private NavMeshObstacle obs;

		public byte needItem = 12;

		[SerializeField]
		private float minRateSound = 3f;

		[SerializeField]
		private float maxRateSound = 5f;

		private AudioSource _as;

		[SerializeField]
		private AudioClip _knockSound;

		[SerializeField]
		private AudioClip _breakSound;

		private float otherFloatWait;

		public float GetRandomTime
		{
			get
			{
				return Random.Range(minRateSound, maxRateSound);
			}
		}

		public bool IsActivated { get; private set; }

		private void Start()
		{
			_as = GetComponent<AudioSource>();
			foreach (Transform item in base.transform)
			{
				item.GetComponent<Collider>().enabled = false;
				item.GetComponent<MeshRenderer>().enabled = false;
			}
			obs.enabled = false;
			otherFloatWait = GetRandomTime;
		}

		private void Update()
		{
			if (otherFloatWait <= 0f)
			{
				otherFloatWait = GetRandomTime;
			}
			else
			{
				otherFloatWait -= Time.deltaTime;
			}
		}

		public void Active()
		{
			foreach (Transform item in base.transform)
			{
				item.GetComponent<MeshRenderer>().enabled = true;
			}
			obs.enabled = true;
			IsActivated = true;
		}

		public void Break()
		{
			if (!IsActivated)
			{
				return;
			}
			obs.enabled = false;
			foreach (Transform item in base.transform)
			{
				item.gameObject.GetComponent<Collider>().enabled = true;
				if (!item.gameObject.GetComponent<Rigidbody>())
				{
					item.gameObject.AddComponent<Rigidbody>();
				}
			}
			PlayBreakSound();
			IsActivated = false;
		}

		public void PlayKnockSound()
		{
			if ((bool)_as && !_as.isPlaying && !(otherFloatWait > 0f))
			{
				_as.clip = _knockSound;
				_as.Play();
			}
		}

		public void PlayBreakSound()
		{
			if ((bool)_as)
			{
				_as.clip = _breakSound;
				_as.Play();
			}
		}
	}
}
