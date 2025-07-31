using Metel.Player;
using UnityEngine;

namespace Metel.Enviroment
{
	public class LatticeTrigger : MonoBehaviour
	{
		private LatticeController _lc;

		private PlayerController _pc;

		private void Start()
		{
			_lc = base.transform.parent.GetComponent<LatticeController>();
			_pc = Object.FindObjectOfType<PlayerController>();
		}

		private void OnTriggerEnter(Collider col)
		{
			if ((bool)_lc && col.tag == "Player")
			{
				_lc.HavePlayer = true;
				if ((bool)_pc)
				{
					_pc.Enter();
				}
			}
		}

		private void OnTriggerExit(Collider col)
		{
			if ((bool)_lc && col.tag == "Player")
			{
				_lc.HavePlayer = false;
				if ((bool)_pc)
				{
					_pc.Exit();
				}
			}
		}
	}
}
