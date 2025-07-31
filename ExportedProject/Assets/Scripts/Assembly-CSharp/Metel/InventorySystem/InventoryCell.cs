using System;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.InventorySystem
{
	[Serializable]
	public struct InventoryCell
	{
		public byte idItem;

		public byte health;

		private Image _cellImage;

		private Image _cellStorke;

		public bool IsEmpty
		{
			get
			{
				return idItem == 0;
			}
		}

		public InventoryCell(Image image, Image storke, byte HP = 0)
		{
			idItem = 0;
			_cellImage = image;
			_cellStorke = storke;
			health = HP;
			Clear();
		}

		public void Damage()
		{
			if (health == 0)
			{
				Clear();
				return;
			}
			health--;
			if (health == 0)
			{
				Clear();
			}
		}

		public void SetSelected(bool value, Color color)
		{
			_cellStorke.color = ((!value) ? new Color(0f, 0f, 0f, 0f) : color);
		}

		public void Clear()
		{
			idItem = 0;
			_cellImage.color = new Color(0f, 0f, 0f, 0f);
		}

		public void SetItem(byte id, byte Health = 0)
		{
			idItem = id;
			health = Health;
			_loadImage();
		}

		private void _loadImage()
		{
			Sprite sprite = Resources.Load<Sprite>("Items/" + idItem);
			if (!_cellImage)
			{
				Debug.LogError("Inventory Cell not initalized.");
				return;
			}
			if (sprite == null)
			{
				Debug.LogError("Item image is not loaded. Item ID: [" + idItem + "]");
				return;
			}
			_cellImage.sprite = sprite;
			_cellImage.color = Color.white;
		}
	}
}
