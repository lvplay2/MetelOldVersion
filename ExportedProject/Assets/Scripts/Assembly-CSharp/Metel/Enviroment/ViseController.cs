using System.Collections;
using UnityEngine;

namespace Metel.Enviroment
{
	public class ViseController : MonoBehaviour
	{
		public byte NeedItem = 11;

		public GameObject needItemVisual;

		public GameObject resultItem;

		private Animator _anims;

		public bool CanBeUse
		{
			get
			{
				return needItemVisual.activeInHierarchy;
			}
		}

		private void Start()
		{
			_anims = base.transform.parent.GetComponent<Animator>();
			needItemVisual.SetActive(false);
		}

		public void SetItem()
		{
			needItemVisual.SetActive(true);
		}

		public void Use()
		{
			if (CanBeUse)
			{
				if ((bool)_anims)
				{
					_anims.SetTrigger("use");
				}
				StartCoroutine(waitToEnd(0.2f));
			}
		}

		private IEnumerator waitToEnd(float time)
		{
			yield return new WaitForSeconds(time);
			if ((bool)resultItem)
			{
				resultItem.layer = 0;
				resultItem.GetComponent<Rigidbody>().isKinematic = false;
			}
			base.gameObject.layer = 2;
			Object.Destroy(this);
		}
	}
}
