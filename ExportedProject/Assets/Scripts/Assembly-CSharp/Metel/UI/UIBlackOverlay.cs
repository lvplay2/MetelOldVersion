using UnityEngine;

namespace Metel.UI
{
	public class UIBlackOverlay : MonoBehaviour
	{
		private Animation anim;

		[SerializeField]
		private AnimationClip setBlack;

		[SerializeField]
		private AnimationClip setNormal;

		private void Start()
		{
			anim = GetComponent<Animation>();
			Set(false);
		}

		public void Set(bool needBlack)
		{
			anim.clip = ((!needBlack) ? setNormal : setBlack);
			anim.Play();
		}
	}
}
