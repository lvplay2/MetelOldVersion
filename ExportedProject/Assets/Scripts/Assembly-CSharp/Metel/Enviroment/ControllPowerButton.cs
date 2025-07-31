using UnityEngine;

namespace Metel.Enviroment
{
	public class ControllPowerButton : MonoBehaviour
	{
		private Animation _anims;

		private ControllPanel _panel;

		public bool IsPowered
		{
			get
			{
				return Object.FindObjectOfType<IsolationController>() == null;
			}
		}

		private void Start()
		{
			_panel = Object.FindObjectOfType<ControllPanel>();
			if ((bool)base.transform.parent.GetComponent<Animation>())
			{
				_anims = base.transform.parent.GetComponent<Animation>();
			}
		}

		public void Click()
		{
			if (IsPowered)
			{
				if ((bool)_anims)
				{
					_anims.Play();
				}
				_panel.isPower = !_panel.isPower;
				base.transform.parent.GetComponent<ButtonHelper>().Play();
			}
		}
	}
}
