using System;
using System.Collections.Generic;
using UnityEngine;

namespace Metel.Localization
{
	public static class MessagesLocalization
	{
		private static List<Item> items = new List<Item>();

		public static Item GetItem(byte id)
		{
			for (byte b = 0; b < items.Count; b++)
			{
				if (items[b].ID == id)
				{
					return items[b];
				}
			}
			return new Item(byte.MaxValue, "Не найдено название!", "Not finded name!");
		}

		public static void LoadItemsLocalization()
		{
			items = new List<Item>();
			TextAsset textAsset = Resources.Load<TextAsset>("Localization");
			string[] array = textAsset.text.Split(new string[1] { "\n" }, StringSplitOptions.None);
			for (byte b = 0; b < array.Length; b++)
			{
				string[] array2 = array[b].Split(',');
				Item item = new Item(Convert.ToByte(array2[0]), array2[1], array2[2]);
				items.Add(item);
			}
		}
	}
}
