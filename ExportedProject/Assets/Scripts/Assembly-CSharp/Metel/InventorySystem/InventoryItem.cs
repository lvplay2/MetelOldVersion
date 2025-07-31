using UnityEngine;

namespace Metel.InventorySystem
{
	public class InventoryItem : MonoBehaviour
	{
		public byte idItem = 1;

		public byte Health;

		public GameObject[] blockedBy;

		public bool AddRigidbody;

		public bool CanBePickup
		{
			get
			{
				if (blockedBy.Length > 0)
				{
					for (byte b = 0; b < blockedBy.Length; b++)
					{
						if (blockedBy[b] != null)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private void Update()
		{
			if (AddRigidbody && CanBePickup && !GetComponent<Rigidbody>())
			{
				base.gameObject.AddComponent<Rigidbody>();
			}
		}
	}
}
