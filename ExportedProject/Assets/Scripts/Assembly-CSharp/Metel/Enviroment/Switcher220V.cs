using UnityEngine;

namespace Metel.Enviroment
{
	public class Switcher220V : MonoBehaviour
	{
		private Animator _anims;

		[SerializeField]
		private GameObject lighining;

		private ElevatorController _elevator;

		public bool IsActivated { get; private set; }

		private void Start()
		{
			_anims = GetComponent<Animator>();
			_elevator = Object.FindObjectOfType<ElevatorController>();
		}

		private void Update()
		{
			if ((bool)_anims)
			{
				_anims.SetBool("activate", IsActivated);
			}
			if ((bool)lighining)
			{
				lighining.SetActive(IsActivated);
			}
		}

		public void Switch()
		{
			IsActivated = !IsActivated;
			if (!IsActivated)
			{
				_elevator.ChangeDirection(false, true);
			}
		}
	}
}
