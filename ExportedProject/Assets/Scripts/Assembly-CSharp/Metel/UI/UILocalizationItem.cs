using Metel.Localization;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.UI
{
	public class UILocalizationItem : MonoBehaviour
	{
		public byte id_localizate_item;

		private void Start()
		{
			if ((bool)GetComponent<Text>())
			{
				GetComponent<Text>().text = MessagesLocalization.GetItem(id_localizate_item).GetFromLanguage(GlobalLocalization.currentLanguage);
			}
		}
	}
}
