using Metel.Player;
using UnityEngine;

namespace Metel.Enviroment
{
	public class ElevatorTrigger : MonoBehaviour
	{
		private PlayerController player;

		private void Start()
		{
			player = Object.FindObjectOfType<PlayerController>();
		}

		private void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == "Player")
			{
				player.SetActiveCrouch(false);
			}
		}

		private void OnTriggerExit(Collider col)
		{
			if (col.gameObject.tag == "Player")
			{
				player.SetActiveCrouch(true);
			}
		}
	}
}
