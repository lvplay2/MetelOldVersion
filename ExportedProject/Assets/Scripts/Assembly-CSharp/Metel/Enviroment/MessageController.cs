using Metel.InventorySystem;
using Metel.Localization;
using UnityEngine;

namespace Metel.Enviroment
{
	public class MessageController : MonoBehaviour
	{
		public byte MessageID = 1;

		public void Use()
		{
			Object.FindObjectOfType<Inventory>().log(MessagesLocalization.GetItem(MessageID).GetFromLanguage(GlobalLocalization.currentLanguage));
		}
	}
}
