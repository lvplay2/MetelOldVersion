using UnityEngine;

namespace Metel.CutScene
{
	public class UIHelper : MonoBehaviour
	{
		[SerializeField]
		private Animation _blackOverlay;

		public void PlayOverlayAnim()
		{
			if ((bool)_blackOverlay)
			{
				_blackOverlay.Play();
			}
		}
	}
}
