using UnityEngine;

namespace Metel.Enviroment
{
	public class ButtonHelper : MonoBehaviour
	{
		private AudioSource _as;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		public void Play()
		{
			if ((bool)_as && !_as.isPlaying)
			{
				_as.Play();
			}
		}
	}
}
