using Metel.Bot;
using UnityEngine;

namespace Metel.Enviroment
{
	public class GlassController : MonoBehaviour
	{
		public GameObject destroyEffect;

		public byte NeedItem = 10;

		public GameObject part;

		private AudioSource _as;

		public AudioClip destroySound;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		public void Destroy()
		{
			PlaySound();
			Object.FindObjectOfType<BotLogic>().Noise = true;
			Object.Destroy(part.gameObject);
			Object.Instantiate(destroyEffect, part.transform.position, Quaternion.identity);
			if ((bool)GetComponent<Collider>())
			{
				GetComponent<Collider>().enabled = false;
			}
		}

		private void PlaySound()
		{
			if ((bool)_as)
			{
				_as.clip = destroySound;
				_as.Play();
			}
		}
	}
}
