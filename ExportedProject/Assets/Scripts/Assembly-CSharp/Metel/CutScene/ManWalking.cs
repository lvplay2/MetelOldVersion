using UnityEngine;

namespace Metel.CutScene
{
	public class ManWalking : MonoBehaviour
	{
		public float speedWalk = 1f;

		public float speedRun = 1.5f;

		private Animator _anims;

		[SerializeField]
		private Transform maniak;

		private bool tmp;

		private void Start()
		{
			_anims = GetComponent<Animator>();
		}

		private void Update()
		{
			bool flag = _anims.GetCurrentAnimatorStateInfo(0).IsName("SwaggerWalk");
			bool flag2 = _anims.GetCurrentAnimatorStateInfo(0).IsName("Running");
			base.transform.position += base.transform.forward * Time.deltaTime * (flag ? speedWalk : ((!flag2) ? 0f : speedRun));
		}

		public void EnableEnemy()
		{
			maniak.GetComponent<ManiakCutScene>().PlayerVisibled();
			tmp = true;
		}

		private void FixedUpdate()
		{
			if (tmp && Vector3.Distance(base.transform.position, maniak.position) > 1.5f)
			{
				Vector3 position = maniak.position;
				position.y = base.transform.position.y;
				base.transform.LookAt(position);
			}
		}
	}
}
