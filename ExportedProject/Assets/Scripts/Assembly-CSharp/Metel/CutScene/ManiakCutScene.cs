using UnityEngine;
using UnityEngine.AI;

namespace Metel.CutScene
{
	public class ManiakCutScene : MonoBehaviour
	{
		[SerializeField]
		private Animator _manAnims;

		private Animator _maniakAnims;

		[SerializeField]
		private Transform player;

		private bool tmp;

		private NavMeshAgent _agent;

		private void Start()
		{
			_agent = GetComponent<NavMeshAgent>();
			_maniakAnims = GetComponent<Animator>();
		}

		private void Update()
		{
			if (tmp)
			{
				if (Vector3.Distance(base.transform.position, player.position) < 1.5f)
				{
					_agent.isStopped = true;
					_maniakAnims.SetTrigger("attack");
				}
				else
				{
					_agent.SetDestination(player.position);
				}
			}
			else
			{
				_maniakAnims.SetBool("angry", false);
			}
		}

		public void PlayerVisibled()
		{
			if (!tmp)
			{
				_maniakAnims.SetBool("angry", true);
				tmp = true;
			}
		}

		public void Damage()
		{
			_manAnims.SetBool("hit", true);
		}
	}
}
