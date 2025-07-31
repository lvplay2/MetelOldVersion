using UnityEngine;

namespace Metel.Enviroment
{
	public class StapleController : MonoBehaviour
	{
		public byte needItem = 5;

		public void Unscrew()
		{
			Object.Destroy(base.gameObject);
		}
	}
}
