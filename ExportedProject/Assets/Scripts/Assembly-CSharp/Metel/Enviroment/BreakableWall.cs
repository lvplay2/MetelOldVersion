using System;
using System.Collections;
using Metel.Bot;
using UnityEngine;

namespace Metel.Enviroment
{
	public class BreakableWall : MonoBehaviour
	{
		public Vector3 breakDirection = Vector3.forward;

		public byte NeedItemID = 2;

		private static float TimeBeforeDisableColliders = 2.1f;

		private AudioSource _as;

		private void Start()
		{
			_as = GetComponent<AudioSource>();
		}

		public void Break()
		{
			PlaySound();
			foreach (Transform item in base.transform)
			{
				if ((bool)item.GetComponent<Collider>())
				{
					item.GetComponent<Collider>().enabled = true;
				}
				if ((bool)item.GetComponent<Rigidbody>())
				{
					item.GetComponent<Rigidbody>().isKinematic = false;
					item.GetComponent<Rigidbody>().AddForce(breakDirection);
				}
			}
			if ((bool)GetComponent<Collider>())
			{
				GetComponent<Collider>().enabled = false;
			}
			UnityEngine.Object.FindObjectOfType<BotLogic>().NoiseAfterBreakWall = true;
			StartCoroutine(DisableColliders(TimeBeforeDisableColliders));
		}

		private IEnumerator DisableColliders(float g)
		{
			yield return new WaitForSeconds(g);
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					if ((bool)transform.GetComponent<Collider>())
					{
						transform.GetComponent<Collider>().enabled = false;
					}
					if ((bool)transform.GetComponent<Rigidbody>())
					{
						transform.GetComponent<Rigidbody>().isKinematic = true;
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				IDisposable disposable = (disposable2 = enumerator as IDisposable);
				if (disposable2 != null)
				{
					disposable.Dispose();
				}
			}
		}

		private void PlaySound()
		{
			if ((bool)_as)
			{
				_as.Play();
			}
		}
	}
}
