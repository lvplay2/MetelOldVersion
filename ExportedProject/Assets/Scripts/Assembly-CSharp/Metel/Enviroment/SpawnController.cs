using System;
using UnityEngine;

namespace Metel.Enviroment
{
	public class SpawnController : MonoBehaviour
	{
		[Serializable]
		public struct SpawnStruct
		{
			public string Tag;

			public GameObject[] target;

			public void EnableRandom()
			{
				if (target.Length != 0)
				{
					int num = UnityEngine.Random.Range(0, target.Length);
					for (int i = 0; i < target.Length; i++)
					{
						target[i].SetActive(i == num);
					}
				}
			}
		}

		public SpawnStruct[] randomObjects;

		private void Start()
		{
			for (int i = 0; i < randomObjects.Length; i++)
			{
				randomObjects[i].EnableRandom();
			}
		}
	}
}
