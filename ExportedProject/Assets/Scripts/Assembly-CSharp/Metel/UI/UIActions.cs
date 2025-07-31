using UnityEngine;
using UnityEngine.SceneManagement;

namespace Metel.UI
{
	public class UIActions : MonoBehaviour
	{
		public const byte ID_LEVEL_GAME = 2;

		public const byte ID_LEVEL_LOADING = 1;

		public const byte ID_LEVEL_MENU = 0;

		public void Play()
		{
			SceneManager.LoadScene(1);
		}

		public void Menu()
		{
			SceneManager.LoadScene(0);
		}

		public void ReloadLevel()
		{
			SceneManager.LoadScene(1);
		}

		public void Quit()
		{
			Application.Quit();
		}
	}
}
