using UnityEngine;

namespace Metel.UI
{
	public class MarkerController : MonoBehaviour
	{
		public MeshRenderer marker;

		public float offset = 0.5f;

		private void Start()
		{
			if (!marker)
			{
				for (byte b = 0; b < base.transform.childCount; b++)
				{
					if (base.transform.GetChild(b).name.ToLower().Contains("marker"))
					{
						marker = base.transform.GetChild(b).GetComponent<MeshRenderer>();
					}
				}
			}
			marker.enabled = false;
			if ((bool)GetComponent<SphereCollider>())
			{
				GetComponent<SphereCollider>().radius = 3f;
			}
		}

		private void FixedUpdate()
		{
			base.transform.position = base.transform.parent.position + Vector3.up * offset;
		}

		public void OnTriggerEnter(Collider col)
		{
			if (col.gameObject.tag == "Player")
			{
				marker.enabled = true;
			}
		}

		public void OnTriggerExit(Collider col)
		{
			if (col.gameObject.tag == "Player")
			{
				marker.enabled = false;
			}
		}
	}
}
