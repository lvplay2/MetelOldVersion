using System;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.UI
{
	public class UIMainMenu : MonoBehaviour
	{
		[Serializable]
		public struct TabStruct
		{
			public GameObject page;

			public byte ID;

			public bool Activated
			{
				get
				{
					return page != null && page.activeSelf;
				}
			}

			public void Click()
			{
				page.SetActive(!page.activeSelf);
			}

			public void SetActive(bool value)
			{
				page.SetActive(value);
			}
		}

		public TabStruct[] Tabs;

		[SerializeField]
		private Slider _music;

		[SerializeField]
		private Slider _effects;

		[SerializeField]
		private Slider _sens;

		[SerializeField]
		private GameObject MetelTab;

		private float timePlay;

		private void Start()
		{
			LoadSettings();
		}

		private void Update()
		{
			if (timePlay < 0.5f)
			{
				timePlay += Time.deltaTime;
			}
		}

		public void TabActivate(int ID)
		{
			for (byte b = 0; b < Tabs.Length; b++)
			{
				Tabs[b].SetActive(b == ID && !Tabs[b].page.activeSelf);
			}
			ReloadMainPage();
		}

		public void CloseAll()
		{
			TabActivate(-1);
			ReloadMainPage();
		}

		public void ReloadMainPage()
		{
			for (byte b = 0; b < Tabs.Length; b++)
			{
				if (Tabs[b].Activated)
				{
					MetelTab.SetActive(false);
					return;
				}
			}
			MetelTab.SetActive(true);
		}

		public void SaveSettings()
		{
			if (!(timePlay < 0.5f))
			{
				PlayerPrefs.SetFloat("Music", _music.value);
				PlayerPrefs.SetFloat("Effects", _effects.value);
				PlayerPrefs.SetFloat("Sensetive", _sens.value);
			}
		}

		public void LoadSettings()
		{
			if (PlayerPrefs.HasKey("Sensetive"))
			{
				_music.value = PlayerPrefs.GetFloat("Music");
				_effects.value = PlayerPrefs.GetFloat("Effects");
				_sens.value = PlayerPrefs.GetFloat("Sensetive");
			}
			else
			{
				PlayerPrefs.SetFloat("Music", _music.value);
				PlayerPrefs.SetFloat("Effects", _effects.value);
				PlayerPrefs.SetFloat("Sensetive", _sens.value);
			}
		}
	}
}
