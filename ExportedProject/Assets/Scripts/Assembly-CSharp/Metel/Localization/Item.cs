namespace Metel.Localization
{
	public class Item
	{
		public byte ID;

		public string Rus;

		public string Eng;

		public Item(byte id, string ru, string eng)
		{
			ID = id;
			Rus = ru;
			Eng = eng;
		}

		public string GetFromLanguage(LanguageType lang)
		{
			switch (lang)
			{
			case LanguageType.ru:
				return Rus;
			case LanguageType.eng:
				return Eng;
			default:
				return "Not finded language type";
			}
		}
	}
}
