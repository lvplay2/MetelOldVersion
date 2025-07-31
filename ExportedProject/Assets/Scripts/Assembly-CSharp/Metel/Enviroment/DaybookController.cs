using UnityEngine;

namespace Metel.Enviroment
{
	public class DaybookController : MonoBehaviour
	{
		public GameObject ui;

		private AudioSource _as;

		public AudioClip takeSound;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		private void PlaySound()
		{
			if ((bool)_as && !_as.isPlaying)
			{
				_as.clip = takeSound;
				_as.Play();
			}
		}

		public void Use()
		{
			if (!ui.activeSelf)
			{
				ui.SetActive(true);
				PlaySound();
			}
		}
	}
}
