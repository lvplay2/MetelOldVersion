using UnityEngine;

namespace Metel.Enviroment
{
	public class ElevatorMechanism : MonoBehaviour
	{
		public byte needItem = 13;

		public GameObject itemVisual;

		private Animator _anims;

		private ElevatorController elevator;

		public bool CanBeUse
		{
			get
			{
				return itemVisual.activeInHierarchy;
			}
		}

		private void Start()
		{
			itemVisual.SetActive(false);
			_anims = base.transform.parent.GetComponent<Animator>();
			elevator = Object.FindObjectOfType<ElevatorController>();
		}

		public void SetItem()
		{
			itemVisual.SetActive(true);
		}

		public void Use()
		{
			if ((bool)_anims)
			{
				_anims.SetTrigger("use");
			}
			elevator.PlaySound(false);
			elevator.ChangeDirection(false);
			Object.Destroy(this);
		}
	}
}
