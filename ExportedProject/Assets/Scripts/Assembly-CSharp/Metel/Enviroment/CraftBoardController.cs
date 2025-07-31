using System;
using UnityEngine;

namespace Metel.Enviroment
{
	public class CraftBoardController : MonoBehaviour
	{
		[Serializable]
		public struct StageStruct
		{
			public GameObject part;

			public byte needItem;

			public bool Completed { get; private set; }

			public void Complete()
			{
				setActive(true);
				Completed = true;
			}

			public void Hide()
			{
				setActive(false);
			}

			private void setActive(bool value)
			{
				if (part == null)
				{
					Debug.LogError("Part is null!");
				}
				else
				{
					part.SetActive(value);
				}
			}
		}

		public GameObject itemResult;

		public StageStruct[] stages;

		private void Start()
		{
			for (byte b = 0; b < stages.Length; b++)
			{
				stages[b].Hide();
			}
		}

		public byte StagesCompleted()
		{
			byte b = 0;
			for (byte b2 = 0; b2 < stages.Length; b2++)
			{
				if (stages[b2].Completed)
				{
					b++;
				}
			}
			return b;
		}

		public bool NeedItem(byte ID)
		{
			for (byte b = 0; b < stages.Length; b++)
			{
				if (stages[b].needItem == ID && !stages[b].Completed)
				{
					return true;
				}
			}
			return false;
		}

		public void Craft(byte ID)
		{
			byte b = 0;
			for (b = 0; b < stages.Length; b++)
			{
				if (stages[b].needItem == ID && !stages[b].Completed)
				{
					stages[b].Complete();
				}
			}
			if (StagesCompleted() == stages.Length)
			{
				for (b = 0; b < stages.Length; b++)
				{
					stages[b].Hide();
				}
				itemResult.SetActive(true);
				UnityEngine.Object.Destroy(this);
			}
		}
	}
}
