using Metel.Bot;
using Metel.Statistics;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.Enviroment
{
	public class ControllPanel : MonoBehaviour
	{
		public bool isPower;

		public static string PASSWORD = "1987";

		public GameObject uiObject;

		public GameObject[] toDisableUI;

		public GameObject loginPage;

		public GameObject controllerPage;

		public InputField passwordField;

		private Material _mat;

		private byte wrongPass;

		[SerializeField]
		private Text consoleText;

		private Switcher220V _switcher;

		private bool tmpStats;

		private AudioSource _as;

		public AudioClip alertSound;

		public bool active { get; private set; }

		public bool IsOpenedPanel
		{
			get
			{
				return uiObject.activeSelf;
			}
		}

		private void Start()
		{
			_as = GetComponent<AudioSource>();
			_mat = GetComponent<MeshRenderer>().material;
			_switcher = Object.FindObjectOfType<Switcher220V>();
		}

		private void Update()
		{
			_mat.SetFloat("_On", isPower ? 1 : 0);
			if (!tmpStats && isPower)
			{
				AmplitudeStats.SendLog("Enable Controll Panel");
				tmpStats = true;
			}
		}

		private void PlaySound()
		{
			if ((bool)_as)
			{
				_as.clip = alertSound;
				_as.Play();
			}
		}

		public void SetActivePanel()
		{
			if (isPower)
			{
				active = !active;
				uiObject.SetActive(active);
				for (byte b = 0; b < toDisableUI.Length; b++)
				{
					toDisableUI[b].SetActive(!active);
				}
				if (!active)
				{
					controllerPage.SetActive(false);
					loginPage.SetActive(true);
				}
			}
		}

		public void CheckLogin()
		{
			if (PASSWORD == passwordField.text)
			{
				controllerPage.SetActive(true);
				loginPage.SetActive(false);
				return;
			}
			wrongPass++;
			if (wrongPass == 3)
			{
				PlaySound();
				Object.FindObjectOfType<BotLogic>().Noise = true;
				wrongPass = 0;
			}
		}

		public void SwitchDoor(bool value)
		{
			if (!Object.FindObjectOfType<DoorController>())
			{
				Debug.LogError("Door Controller game object is not finded.");
			}
			else
			{
				Object.FindObjectOfType<DoorController>().ChangeDirection(value);
			}
		}

		public void SwitchElevator(bool value)
		{
			if (!_switcher.IsActivated)
			{
				consoleText.text = "Need power...";
			}
			else
			{
				consoleText.text = "Success...";
			}
			if (!Object.FindObjectOfType<ElevatorController>())
			{
				Debug.LogError("Elevator Controller game object is not finded.");
			}
			else
			{
				Object.FindObjectOfType<ElevatorController>().ChangeDirection(value);
			}
		}
	}
}
