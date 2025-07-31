using UnityEngine;

namespace Metel.UI
{
	public class UIController : MonoBehaviour
	{
		public GameObject[] inGameUI;

		public GameObject controllPanelUI;

		public GameObject pauseUI;

		public UIStateType currentState { get; private set; }

		public void SwitchState(UIStateType target)
		{
			if (currentState != target)
			{
				switch (target)
				{
				default:
					Debug.LogError("Game state is unknown...");
					break;
				case UIStateType.Game:
				case UIStateType.ControllPanel:
				case UIStateType.Pause:
					break;
				}
				_setActiveGameUI(currentState == UIStateType.Game);
				pauseUI.SetActive(currentState == UIStateType.Pause);
				controllPanelUI.SetActive(currentState == UIStateType.ControllPanel);
			}
		}

		private void _setActiveGameUI(bool value)
		{
			for (byte b = 0; b < inGameUI.Length; b++)
			{
				inGameUI[b].SetActive(value);
			}
		}
	}
}
