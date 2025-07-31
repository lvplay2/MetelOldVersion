using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Metel.Audio
{
	public class AudioController : MonoBehaviour
	{
		public TypeAudio currentAudio;

		[SerializeField]
		private AudioSource _sourceDefault;

		[SerializeField]
		private AudioSource _sourceWarning;

		[SerializeField]
		private AudioSource _sourceDanger;

		private AudioMixer _musicDefault;

		private AudioMixer _musicWarning;

		private AudioMixer _musicDanger;

		[SerializeField]
		private AudioMixer _mixer;

		[SerializeField]
		private float timeSmooth = 1f;

		public const string DefaultMusicGroup = "musicDefault";

		public const string DangerMusicGroup = "musicDanger";

		public const string WarningMusicGroup = "musicWarning";

		private bool tmp;

		private float MaximumVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Music", 0.8f);
			}
		}

		private float MaximumEffectsVolume
		{
			get
			{
				return PlayerPrefs.GetFloat("Effects", 0.8f);
			}
		}

		private void Start()
		{
			_sourceDefault = base.transform.GetChild(0).GetComponent<AudioSource>();
			_sourceWarning = base.transform.GetChild(1).GetComponent<AudioSource>();
			_sourceDanger = base.transform.GetChild(2).GetComponent<AudioSource>();
			_musicDefault = _mixer.FindMatchingGroups("MusicDefault")[0].audioMixer;
			_musicWarning = _mixer.FindMatchingGroups("MusicWarning")[0].audioMixer;
			_musicDanger = _mixer.FindMatchingGroups("MusicDanger")[0].audioMixer;
			_mixer.SetFloat("Music", MaximumVolume);
			_mixer.SetFloat("Effects", MaximumEffectsVolume);
		}

		public void ChangeMusic(TypeAudio target)
		{
			switch (target)
			{
			case TypeAudio.Warning:
				if (!_sourceWarning.isPlaying)
				{
					_musicDefault.SetFloat("musicDefault", 0f);
					_musicWarning.SetFloat("musicWarning", 1f);
					_sourceWarning.Play();
					_sourceDefault.Stop();
				}
				break;
			case TypeAudio.Danger:
				if (!_sourceDanger.isPlaying)
				{
					_musicDefault.SetFloat("musicDefault", 0f);
					_musicDanger.SetFloat("musicDanger", 1f);
					_sourceDanger.Play();
					_sourceDefault.Stop();
				}
				break;
			case TypeAudio.Music:
				StartCoroutine(SetDefaultMusic(timeSmooth));
				break;
			default:
				Debug.LogError(target.ToString() + " is wrong");
				break;
			}
		}

		private IEnumerator SetDefaultMusic(float time)
		{
			if (tmp)
			{
				yield return null;
			}
			tmp = true;
			float o = 0f;
			if (!_sourceDefault.isPlaying)
			{
				_sourceDefault.Play();
			}
			_musicDefault.GetFloat("musicDefault", out o);
			_musicDefault.SetFloat("musicDefault", Mathf.Lerp(o, MaximumVolume, time * Time.deltaTime));
			yield return new WaitForSeconds(time);
			tmp = false;
		}
	}
}
