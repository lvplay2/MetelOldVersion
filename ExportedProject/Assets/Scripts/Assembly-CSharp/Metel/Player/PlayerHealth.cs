using System.Collections;
using Metel.Bot;
using Metel.Enviroment;
using Metel.Statistics;
using Metel.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.Player
{
	public class PlayerHealth : MonoBehaviour
	{
		public byte CurrentHealth = 3;

		[SerializeField]
		private Transform vatherUIStates;

		public FP_Controller _controller;

		private Image image;

		private LatticeController _lc;

		private UIBlackOverlay overlay;

		private StressReceiver _stress;

		[SerializeField]
		private GameObject _effectElectrocity;

		private Wardrobe wardrobe;

		private Vector3 lookAtTarget;

		[SerializeField]
		private float SmoothLookAt = 50f;

		[SerializeField]
		private GameObject looseUI;

		private PlayerController _pc;

		[SerializeField]
		private GameObject[] UI_CONTROLL_ELEMENTS;

		public bool IsLatticeNear
		{
			get
			{
				return (bool)_lc && Vector3.Distance(base.transform.position, _lc.transform.position) < 1.5f;
			}
		}

		private void Start()
		{
			image = GameObject.Find("BlackImage").GetComponent<Image>();
			image.enabled = false;
			_lc = Object.FindObjectOfType<LatticeController>();
			overlay = Object.FindObjectOfType<UIBlackOverlay>();
			_stress = Object.FindObjectOfType<StressReceiver>();
			wardrobe = Object.FindObjectOfType<Wardrobe>();
			_pc = Object.FindObjectOfType<PlayerController>();
		}

		private void Update()
		{
			if (lookAtTarget != Vector3.zero)
			{
				Transform child = base.transform.GetChild(0);
				Vector3 forward = lookAtTarget - child.position;
				Quaternion to = Quaternion.LookRotation(forward);
				child.rotation = Quaternion.RotateTowards(child.rotation, to, SmoothLookAt * Time.deltaTime);
			}
		}

		public void Damage(float timeProcess = 3f)
		{
			if (IsLatticeNear)
			{
				StartCoroutine(DamageProcess(timeProcess));
			}
		}

		private IEnumerator DamageProcess(float timeProcess = 1.5f)
		{
			_effectElectrocity.SetActive(true);
			overlay.Set(true);
			_stress.InduceStress(1f);
			_lc.SetActiveEffect(true);
			yield return new WaitForSeconds(timeProcess);
			wardrobe.Reset();
			_effectElectrocity.SetActive(false);
			_lc.SetActiveEffect(false);
			CurrentHealth--;
			_lc.NextState();
			for (byte b = 0; b < vatherUIStates.childCount; b++)
			{
				vatherUIStates.GetChild(b).gameObject.SetActive(vatherUIStates.GetChild(b).name == CurrentHealth.ToString());
			}
			if (CurrentHealth == 0)
			{
				Kill();
			}
			overlay.Set(false);
		}

		public void Kill()
		{
			if ((bool)_pc)
			{
				_pc.StandUp();
			}
			image.enabled = true;
			Object.FindObjectOfType<PlayerController>().Loose();
			StartCoroutine(DrawLooseUI(1f));
			Object.FindObjectOfType<TimeInPlay>().ClearCurrentGameTime();
		}

		private IEnumerator DrawLooseUI(float time)
		{
			yield return new WaitForSeconds(time);
			looseUI.SetActive(true);
		}

		public void LookAtOn(Vector3 point, bool blockInput = true)
		{
			if (blockInput)
			{
				for (byte b = 0; b < UI_CONTROLL_ELEMENTS.Length; b++)
				{
					UI_CONTROLL_ELEMENTS[b].SetActive(false);
				}
			}
			lookAtTarget = point;
		}
	}
}
