using Metel.Localization;
using UnityEngine;

namespace Metel
{
	public class StaticController : MonoBehaviour
	{
		private void Start()
		{
			ItemLocalization.LoadItemsLocalization();
			MessagesLocalization.LoadItemsLocalization();
		}
	}
}
