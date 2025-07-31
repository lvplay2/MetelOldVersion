using System.Collections;
using UnityEngine;

namespace Metel.Enviroment
{
	public class FaucetObject : MonoBehaviour
	{
		public float smoothTime = 2.5f;

		public Vector3 endRotate;

		private bool tmp;

		public GameObject _water;

		private AudioSource _as;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		private void Update()
		{
			if (tmp)
			{
				base.transform.localRotation = Quaternion.Lerp(base.transform.localRotation, Quaternion.Euler(endRotate), smoothTime * Time.deltaTime);
			}
		}

		public void Activate()
		{
			if (!tmp)
			{
				if ((bool)_as)
				{
					_as.Play();
				}
				tmp = true;
				if (_water != null)
				{
					Object.Destroy(_water);
				}
				StartCoroutine(Rotate(smoothTime));
			}
		}

		private IEnumerator Rotate(float time)
		{
			yield return new WaitForSeconds(time);
			base.transform.localRotation = Quaternion.Euler(endRotate);
			Object.Destroy(this);
		}
	}
}
