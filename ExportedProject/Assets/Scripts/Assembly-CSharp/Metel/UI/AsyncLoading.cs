using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Metel.UI
{
	public class AsyncLoading : MonoBehaviour
	{
		[SerializeField]
		private float speedRotate = 15f;

		[SerializeField]
		private Transform circle;

		[SerializeField]
		private GameObject textTap;

		private bool isDone;

		private AsyncOperation asyncLoad;

		[SerializeField]
		private GameObject texts;

		private void Start()
		{
			isDone = false;
			asyncLoad = SceneManager.LoadSceneAsync(2);
			asyncLoad.allowSceneActivation = false;
			StartCoroutine(Load());
		}

		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				asyncLoad.allowSceneActivation = true;
			}
		}

		private IEnumerator Load()
		{
			while (!asyncLoad.isDone)
			{
				if (asyncLoad.progress >= 0.9f)
				{
					textTap.SetActive(true);
					texts.SetActive(false);
				}
				if ((bool)circle)
				{
					circle.transform.localRotation *= Quaternion.Euler(0f, 0f, speedRotate);
				}
				yield return null;
			}
		}
	}
}
