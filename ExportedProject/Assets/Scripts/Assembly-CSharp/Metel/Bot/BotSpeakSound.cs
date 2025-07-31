using System;
using UnityEngine;

namespace Metel.Bot
{
	[Serializable]
	public struct BotSpeakSound
	{
		public string tag;

		public AudioClip[] clip;

		public AudioClip GetClip
		{
			get
			{
				return (clip.Length <= 0) ? null : clip[UnityEngine.Random.Range(0, clip.Length)];
			}
		}
	}
}
