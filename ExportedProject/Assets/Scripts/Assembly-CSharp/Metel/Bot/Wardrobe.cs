using Metel.Enviroment;
using UnityEngine;

namespace Metel.Bot
{
	public class Wardrobe : MonoBehaviour
	{
		public bool[] OpenWardrobe = new bool[0];

		public GameObject[] WardrobeObj = new GameObject[0];

		private int currentFrame = 5;

		private void Update()
		{
			currentFrame--;
			if (currentFrame <= 0)
			{
				Reload();
				currentFrame = 5;
			}
		}

		private void Reload()
		{
			for (byte b = 0; b < WardrobeObj.Length; b++)
			{
				GameObject gameObject = WardrobeObj[b];
				if ((bool)gameObject.GetComponent<BoxController>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<BoxController>().IsOpened;
				}
				else if ((bool)gameObject.GetComponent<ChestScript>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<ChestScript>().isOpened;
				}
				else if ((bool)gameObject.GetComponent<LatticeController>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<LatticeController>().isOpened;
				}
				else if ((bool)gameObject.GetComponent<DoorController>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<DoorController>().IsOpened;
				}
				else if ((bool)gameObject.GetComponent<ControllPanel>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<ControllPanel>().isPower;
				}
				else if ((bool)gameObject.GetComponent<SmallDoorController>())
				{
					OpenWardrobe[b] = gameObject.GetComponent<SmallDoorController>().IsOpened;
				}
			}
		}

		public void Reset()
		{
			for (byte b = 0; b < WardrobeObj.Length; b++)
			{
				GameObject gameObject = WardrobeObj[b];
				if ((bool)gameObject.GetComponent<BoxController>())
				{
					gameObject.GetComponent<BoxController>().ChangeDirection(false);
				}
				else if ((bool)gameObject.GetComponent<ChestScript>())
				{
					gameObject.GetComponent<ChestScript>().ChangeActive(false);
				}
				else if ((bool)gameObject.GetComponent<LatticeController>())
				{
					gameObject.GetComponent<LatticeController>().ChangeState(false);
				}
				else if ((bool)gameObject.GetComponent<DoorController>())
				{
					gameObject.GetComponent<DoorController>().ChangeDirection(false);
				}
				else if ((bool)gameObject.GetComponent<ControllPanel>())
				{
					gameObject.GetComponent<ControllPanel>().isPower = false;
				}
				else if ((bool)gameObject.GetComponent<SmallDoorController>())
				{
					gameObject.GetComponent<SmallDoorController>().SetOpened(false);
				}
			}
		}
	}
}
