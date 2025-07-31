using System.Collections.Generic;
using Metel.InventorySystem;
using UnityEngine;

namespace Metel.Enviroment
{
	public class ChestScript : MonoBehaviour
	{
		public List<GameObject> _storage;

		public byte IDKey;

		private Collider myCollider;

		private Animator _anims;

		private AudioSource _as;

		public AudioClip openSound;

		public AudioClip closeSound;

		public AudioClip unlockSound;

		public bool isOpened { get; private set; }

		public bool isUnlocked { get; private set; }

		public void Unlock()
		{
			if (!isUnlocked)
			{
				isUnlocked = true;
				PlaySound(true);
			}
		}

		private void Start()
		{
			_as = GetComponent<AudioSource>();
			if ((bool)GetComponent<Collider>())
			{
				myCollider = GetComponent<Collider>();
			}
			if ((bool)GetComponent<Animator>())
			{
				_anims = GetComponent<Animator>();
			}
			_storage = new List<GameObject>();
			byte b = 0;
			for (b = 0; b < base.transform.childCount; b++)
			{
				if ((bool)base.transform.GetChild(b).GetComponent<InventoryItem>())
				{
					_storage.Add(base.transform.GetChild(b).gameObject);
				}
			}
			SetActiveStorage(false);
		}

		private void PlaySound(bool unlock = false)
		{
			if ((bool)_as && !_as.isPlaying)
			{
				_as.clip = (unlock ? unlockSound : ((!isOpened) ? closeSound : openSound));
				_as.Play();
			}
		}

		private void Update()
		{
			if ((bool)_anims)
			{
				_anims.SetBool("isOpened", isOpened);
			}
			if (isUnlocked)
			{
				if (!_isEmptyStorageOrNull() && isOpened)
				{
					if ((bool)myCollider)
					{
						myCollider.enabled = false;
					}
				}
				else if ((bool)myCollider)
				{
					myCollider.enabled = true;
				}
			}
			else if ((bool)myCollider)
			{
				myCollider.enabled = true;
			}
		}

		private bool _isEmptyStorageOrNull()
		{
			if (_storage == null || _storage.Count == 0)
			{
				return true;
			}
			for (byte b = 0; b < _storage.Count; b++)
			{
				if (_storage[b] != null)
				{
					return false;
				}
			}
			return true;
		}

		public void ChangeActive()
		{
			if (isUnlocked)
			{
				isOpened = !isOpened;
				SetActiveStorage(isOpened);
				PlaySound();
			}
		}

		public void ChangeActive(bool value)
		{
			if (isUnlocked)
			{
				isOpened = value;
				SetActiveStorage(isOpened);
				PlaySound();
			}
		}

		private void SetActiveStorage(bool value)
		{
			if (_storage == null)
			{
				Debug.LogError("Storage is null");
				return;
			}
			if (_storage.Count == 0)
			{
				Debug.LogWarning("Storage is empty");
				return;
			}
			for (byte b = 0; b < _storage.Count; b++)
			{
				if (_storage[b] != null)
				{
					_storage[b].SetActive(value);
				}
			}
		}
	}
}
