using UnityEngine;

namespace Metel.Enviroment
{
	public class DoorController : MonoBehaviour
	{
		public Vector3 closedRotation;

		public Vector3 openedRotation;

		public static float SpeedRotate = 120f;

		public bool IsOpened { get; private set; }

		private Vector3 targetRotation
		{
			get
			{
				return (!IsOpened) ? closedRotation : openedRotation;
			}
		}

		public void ChangeDirection(bool value)
		{
			if (Vector3.Distance(base.transform.localRotation.eulerAngles, targetRotation) < 0.1f)
			{
				IsOpened = value;
			}
		}

		private void Start()
		{
		}

		private void Update()
		{
			if (Vector3.Distance(base.transform.localRotation.eulerAngles, targetRotation) > 0.1f)
			{
				base.transform.localRotation = Quaternion.RotateTowards(base.transform.localRotation, Quaternion.Euler(targetRotation), SpeedRotate * Time.deltaTime);
			}
			else
			{
				base.transform.localRotation = Quaternion.Euler(targetRotation);
			}
		}
	}
}
