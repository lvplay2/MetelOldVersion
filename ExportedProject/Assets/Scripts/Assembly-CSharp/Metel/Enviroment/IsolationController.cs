using UnityEngine;

namespace Metel.Enviroment
{
	public class IsolationController : MonoBehaviour
	{
		private MeshRenderer _renderer;

		public byte NeedItem = 3;

		private AudioSource _as;

		private void Start()
		{
			_renderer = GetComponent<MeshRenderer>();
			_renderer.enabled = false;
			_as = GetComponent<AudioSource>();
		}

		public void Use()
		{
			_renderer.enabled = true;
			if ((bool)_as)
			{
				_as.Play();
			}
			if ((bool)Object.FindObjectOfType<ControllPanel>())
			{
				Object.FindObjectOfType<ControllPanel>().isPower = false;
			}
			Object.Destroy(this);
		}
	}
}
