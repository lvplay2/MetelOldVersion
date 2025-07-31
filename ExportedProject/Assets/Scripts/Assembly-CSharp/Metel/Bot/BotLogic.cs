using System.Collections;
using Metel.Audio;
using Metel.Enviroment;
using Metel.Player;
using UnityEngine;
using UnityEngine.AI;

namespace Metel.Bot
{
	public sealed class BotLogic : MonoBehaviour
	{
		public float timeWait = 47f;

		public Transform[] Target = new Transform[0];

		public Wardrobe wardrobe;

		[Range(0f, 5f)]
		public float RotationSpeed;

		public bool Noise;

		public bool NoiseAfterBreakWall;

		public GameObject barInHand;

		public GameObject barOnBarrel;

		public bool DEBUG;

		public PlayerController player;

		public BotSpeakSound[] phrases;

		public AudioSource speaker;

		[SerializeField]
		[Range(0f, 3f)]
		private float CrouchSpeed;

		[SerializeField]
		[Range(0f, 5f)]
		private float MovingSpeed;

		[SerializeField]
		[Range(0f, 7f)]
		private float AngryMovingSpeed;

		private bool AnimPlay;

		private Animator Anim;

		private int NumberTurget;

		private Transform Player;

		private NavMeshAgent agent;

		private float timerWait;

		private bool wait;

		private bool work;

		private bool Finding;

		private AudioController _ac;

		[SerializeField]
		private Animator _timer;

		[SerializeField]
		private AudioSource facePunchAudio;

		private bool tmpPlayed;

		private bool tmp_Played2;

		public bool InRoom
		{
			get
			{
				return base.transform.position.x < 0f;
			}
		}

		private void PlayPhrase(string tag)
		{
			if (!speaker || speaker.isPlaying)
			{
				return;
			}
			for (byte b = 0; b < phrases.Length; b++)
			{
				if (phrases[b].tag == tag)
				{
					speaker.clip = phrases[b].GetClip;
					speaker.Play();
					break;
				}
			}
		}

		private void Start()
		{
			_ac = Object.FindObjectOfType<AudioController>();
			timerWait = timeWait;
			Player = base.gameObject.transform;
			agent = base.gameObject.GetComponent<NavMeshAgent>();
			Anim = base.gameObject.GetComponent<Animator>();
			ResetTimerWait();
		}

		private void SpeedController()
		{
			if (Anim.GetBool("moving"))
			{
				agent.speed = MovingSpeed;
			}
			if (Anim.GetBool("angryMoving"))
			{
				agent.speed = AngryMovingSpeed;
			}
			if (Anim.GetBool("crouch"))
			{
				agent.speed = CrouchSpeed;
			}
			if (DEBUG)
			{
				Debug.Log(agent.speed);
			}
		}

		private void FixedUpdate()
		{
			Move();
			SpeedController();
			if (Finding)
			{
				FindingOpenWardrobe();
			}
		}

		private IEnumerator Wait(float WaitTime)
		{
			work = true;
			yield return new WaitForSeconds(WaitTime);
			wait = true;
			yield return new WaitForFixedUpdate();
			wait = false;
			work = false;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.name == "CrouchTrigger")
			{
				Anim.SetBool("angryMoving", false);
				Anim.SetBool("crouch", true);
			}
			if (other.gameObject.name == "F")
			{
				Anim.SetBool("crouch", false);
				Anim.SetBool("angryMoving", true);
				NumberTurget = 4;
			}
			if (other.gameObject.name == "PlayerTrigger")
			{
				Anim.SetBool("angryMoving", false);
				Anim.SetBool("attack", true);
				Object.FindObjectOfType<PlayerHealth>().LookAtOn(base.transform.position + Vector3.up * 1.5f);
				agent.isStopped = true;
			}
		}

		private void ChooseTarget()
		{
			if (AnimPlay)
			{
				return;
			}
			switch (NumberTurget)
			{
			case 0:
				tmpPlayed = false;
				tmp_Played2 = false;
				Anim.SetBool("moving", false);
				Anim.SetBool("idleMode", true);
				if (timeWait <= 0f)
				{
					if (Noise)
					{
						Anim.SetBool("idleMode", false);
						Anim.SetBool("moving", true);
						NumberTurget++;
					}
					else if (NoiseAfterBreakWall)
					{
						Anim.SetBool("idleMode", false);
						Anim.SetBool("moving", true);
						NumberTurget++;
						if ((bool)barInHand)
						{
							barInHand.SetActive(true);
						}
					}
				}
				else if (Noise || NoiseAfterBreakWall)
				{
					timeWait -= Time.deltaTime;
					if ((bool)_timer)
					{
						_timer.SetBool("near", timeWait < 5f);
					}
				}
				else if ((bool)_timer)
				{
					_timer.SetBool("near", false);
				}
				break;
			case 1:
				Noise = false;
				if (!tmpPlayed)
				{
					PlayPhrase("listenNoise");
					tmpPlayed = true;
				}
				NumberTurget++;
				if (!player.isStay)
				{
					Anim.SetBool("moving", false);
					Anim.SetBool("angryMoving", true);
					NumberTurget = 4;
					PlayPhrase("attack");
				}
				if (NoiseAfterBreakWall)
				{
					Anim.SetBool("idleMode", false);
					Anim.SetBool("moving", true);
					NumberTurget = 3;
				}
				break;
			case 2:
				if (!player.isStay)
				{
					Anim.SetBool("moving", false);
					Anim.SetBool("angryMoving", true);
					NumberTurget = 4;
					PlayPhrase("attack");
					break;
				}
				Anim.SetBool("moving", false);
				Anim.SetBool("idleMode", true);
				if (!work)
				{
					StartCoroutine(Wait(5f));
				}
				if (wait)
				{
					AnimPlay = true;
					Finding = true;
				}
				break;
			case 3:
				if (!NoiseAfterBreakWall)
				{
					Rotation();
					Anim.SetBool("moving", false);
					if (Rotation())
					{
						Anim.SetBool("leverDown", true);
					}
					if (!work)
					{
						StartCoroutine(Wait(0.2f));
						PlayPhrase("volt");
						Object.FindObjectOfType<PlayerHealth>().Damage();
					}
					if (wait)
					{
						Anim.SetBool("leverDown", false);
						Anim.SetBool("moving", true);
						NumberTurget = 0;
						ResetTimerWait();
					}
				}
				else
				{
					if ((bool)barOnBarrel)
					{
						barOnBarrel.SetActive(true);
					}
					if ((bool)barInHand)
					{
						barInHand.SetActive(false);
					}
					Anim.SetBool("moving", true);
					NumberTurget = 2;
				}
				if (!player.isStay)
				{
					Anim.SetBool("moving", false);
					Anim.SetBool("angryMoving", true);
					NumberTurget = 4;
					PlayPhrase("attack");
				}
				NoiseAfterBreakWall = false;
				break;
			}
		}

		private void Move()
		{
			float num = Vector3.Distance(Player.transform.position, Target[NumberTurget].transform.position);
			if (!AnimPlay)
			{
				agent.SetDestination(Target[NumberTurget].transform.position);
			}
			if (num < 0.1f)
			{
				ChooseTarget();
			}
		}

		private bool Rotation()
		{
			if (Player.transform.rotation == Target[NumberTurget].transform.rotation)
			{
				return true;
			}
			Player.transform.rotation = Quaternion.RotateTowards(Player.transform.rotation, Target[NumberTurget].transform.rotation, RotationSpeed);
			return false;
		}

		private void FindingOpenWardrobe()
		{
			bool flag = false;
			for (int i = 0; i < wardrobe.OpenWardrobe.Length; i++)
			{
				if (!wardrobe.OpenWardrobe[i] || !player.isStay)
				{
					continue;
				}
				if (!tmp_Played2)
				{
					if ((bool)wardrobe.WardrobeObj[i].GetComponent<DoorController>())
					{
						PlayPhrase("door");
					}
					else if ((bool)wardrobe.WardrobeObj[i].GetComponent<ChestScript>() || (bool)wardrobe.WardrobeObj[i].GetComponent<BoxController>() || (bool)wardrobe.WardrobeObj[i].GetComponent<SmallDoorController>())
					{
						PlayPhrase("box");
					}
					else if ((bool)wardrobe.WardrobeObj[i].GetComponent<ControllPanel>())
					{
						PlayPhrase("terminal");
					}
					tmp_Played2 = true;
				}
				flag = true;
				if (Rotation())
				{
					Anim.SetBool("idleMode", false);
					Anim.SetBool("angryMode", true);
					if (!work)
					{
						StartCoroutine(Wait(2f));
					}
					if (wait)
					{
						Anim.SetBool("angryMode", false);
						Anim.SetBool("moving", true);
						Finding = false;
						AnimPlay = false;
						NumberTurget = 3;
					}
				}
			}
			if (!flag)
			{
				PlayPhrase("allOk");
				NumberTurget = 0;
				Finding = false;
				AnimPlay = false;
				Anim.SetBool("idleMode", false);
				Anim.SetBool("moving", true);
				ResetTimerWait();
			}
		}

		public void PlayAngryMusic()
		{
			_ac.ChangeMusic(TypeAudio.Danger);
		}

		public void ResetTimerWait()
		{
			timeWait = timerWait;
		}

		public void Damage()
		{
			Object.FindObjectOfType<PlayerHealth>().Kill();
			if ((bool)facePunchAudio)
			{
				facePunchAudio.Play();
			}
		}

		public string GetTimeWait()
		{
			if (Noise || NoiseAfterBreakWall)
			{
				if (timeWait > 0f)
				{
					_ac.ChangeMusic(TypeAudio.Warning);
					int num = (int)timeWait / 60;
					int num2 = (int)timeWait - num * 60;
					return num + ":" + ((num2 >= 10) ? num2.ToString() : ("0" + num2));
				}
				return string.Empty;
			}
			_ac.ChangeMusic(TypeAudio.Music);
			return string.Empty;
		}
	}
}
