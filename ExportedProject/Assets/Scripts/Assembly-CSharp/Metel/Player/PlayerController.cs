using System.Collections;
using Metel.Enviroment;
using Metel.Statistics;
using UnityEngine;

namespace Metel.Player
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField]
		private GameObject moveJoystick;

		[SerializeField]
		private Collider latticeDoorCollider;

		[SerializeField]
		private FP_Button crouchButton;

		private Animator _anims;

		private LatticeController _lc;

		public bool DEBUG;

		[HideInInspector]
		public bool isStay
		{
			get
			{
				if ((bool)_lc)
				{
					return _lc.HavePlayer;
				}
				return false;
			}
		}

		[HideInInspector]
		public bool InLattice { get; private set; }

		public bool IsLoose { get; private set; }

		public bool IsWin { get; private set; }

		public bool IsGame
		{
			get
			{
				return !IsLoose && !IsWin;
			}
		}

		private void Start()
		{
			_anims = GetComponent<Animator>();
			_lc = Object.FindObjectOfType<LatticeController>();
			AmplitudeStats.SendLog("Game Begin");
		}

		public void Action()
		{
			if (InLattice)
			{
				Exit();
			}
			else
			{
				Enter();
			}
		}

		public void Enter()
		{
			SetActiveCrouch(false);
		}

		public void Exit()
		{
			SetActiveCrouch(true);
		}

		private void SetAnimEnter(bool value)
		{
			if (!_anims)
			{
				Debug.LogError("Animator is not finded");
				return;
			}
			_anims.SetBool("in", value);
			InLattice = value;
		}

		public void Loose()
		{
			if (!IsLoose)
			{
				Object.FindObjectOfType<TimeInPlay>().ClearCurrentGameTime();
			}
			IsLoose = true;
		}

		public void Win()
		{
			if (!IsWin)
			{
				Object.FindObjectOfType<TimeInPlay>().ClearCurrentGameTime();
			}
			IsWin = true;
		}

		public void StandUp()
		{
			crouchButton.SetToggle(false);
		}

		private IEnumerator CloseLattice(float waitTime = 2f)
		{
			yield return new WaitForSeconds(waitTime);
			latticeDoorCollider.enabled = true;
			_lc.ChangeState(false);
		}

		public void SetActiveCrouch(bool value)
		{
			if (crouchButton != null)
			{
				crouchButton.gameObject.SetActive(value);
			}
		}
	}
}
