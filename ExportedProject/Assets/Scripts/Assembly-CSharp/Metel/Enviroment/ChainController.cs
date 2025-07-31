using UnityEngine;

namespace Metel.Enviroment
{
	public class ChainController : MonoBehaviour
	{
		public byte needItem = 2;

		private Animation anim;

		private AudioSource _as;

		private void Start()
		{
			anim = GetComponent<Animation>();
			_as = GetComponent<AudioSource>();
		}

		public void Activate()
		{
			anim.Play();
			if ((bool)_as)
			{
				_as.Play();
			}
			if ((bool)GetComponent<Collider>())
			{
				Object.Destroy(GetComponent<Collider>());
			}
			Object.Destroy(base.gameObject, anim.clip.length + _as.clip.length);
		}
	}
}
