using UnityEngine;

namespace Metel.Enviroment
{
	public class ScrewController : MonoBehaviour
	{
		public byte needItem = 10;

		public void Unscrew()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
