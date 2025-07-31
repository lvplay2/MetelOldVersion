using Metel.Bot;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.UI
{
	public class UIBotTimer : MonoBehaviour
	{
		private BotLogic _bot;

		private Text _text;

		private Animator _anims;

		private void Start()
		{
			_bot = Object.FindObjectOfType<BotLogic>();
			_text = GetComponent<Text>();
			_anims = GetComponent<Animator>();
		}

		private void Update()
		{
			_text.text = _bot.GetTimeWait();
			if (!string.IsNullOrEmpty(_text.text))
			{
				_anims.SetBool("draw", true);
			}
			else
			{
				_anims.SetBool("draw", false);
			}
		}
	}
}
