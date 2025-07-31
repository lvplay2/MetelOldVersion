using Metel.Bot;
using Metel.Enviroment;
using Metel.Localization;
using Metel.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Metel.InventorySystem
{
	public class Inventory : MonoBehaviour
	{
		public const float TIME_FOR_TAP = 0.1f;

		public InventoryCell[] inventory;

		public Color _selectedStorkeColor;

		public LayerMask mask;

		public const float LenghtRay = 1.5f;

		public const float TimeUpdate = 15f;

		private int currentFrame;

		private InventoryLogger _logger;

		private float timeTap;

		private PlayerController _pc;

		private BotLogic _bot;

		private bool LockTap;

		[SerializeField]
		private AudioSource _takeSound;

		private ControllPanel _cp;

		public byte GetSelectedCell { get; private set; }

		public sbyte GetEmptyCell
		{
			get
			{
				for (sbyte b = 0; b < inventory.Length; b++)
				{
					if (inventory[b].IsEmpty)
					{
						return b;
					}
				}
				return -1;
			}
		}

		public GameObject GetUnderTap()
		{
			if (LockTap || ((bool)_cp && _cp.IsOpenedPanel))
			{
				return null;
			}
			Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(ray, out hitInfo, 1.5f, mask))
			{
				return hitInfo.collider.gameObject;
			}
			return null;
		}

		private void Awake()
		{
			_pc = Object.FindObjectOfType<PlayerController>();
			_bot = Object.FindObjectOfType<BotLogic>();
		}

		private void Start()
		{
			_cp = Object.FindObjectOfType<ControllPanel>();
			_logger = Object.FindObjectOfType<InventoryLogger>();
			GameObject gameObject = GameObject.Find("InventoryUI");
			if (!gameObject)
			{
				Debug.LogError("Object with name: 'InventoryUI' not finded.");
				return;
			}
			inventory = new InventoryCell[gameObject.transform.childCount];
			for (byte b = 0; b < gameObject.transform.childCount; b++)
			{
				inventory[b] = new InventoryCell(gameObject.transform.GetChild(b).GetChild(0).GetComponent<Image>(), gameObject.transform.GetChild(b).GetChild(1).GetComponent<Image>(), 0);
			}
		}

		public void log(string msg)
		{
			if (!_logger)
			{
				Debug.LogError("Logger is null.");
			}
			else
			{
				_logger.ShowMessage(msg);
			}
		}

		private void Update()
		{
			if (Input.GetMouseButton(0))
			{
				timeTap += Time.deltaTime;
				if (timeTap > 0.1f)
				{
					LockTap = true;
				}
			}
			if (GetUnderTap() != null && Input.GetMouseButtonUp(0) && !LockTap)
			{
				if ((bool)GetUnderTap().GetComponent<InventoryItem>())
				{
					if (GetEmptyCell != -1)
					{
						InventoryItem component = GetUnderTap().GetComponent<InventoryItem>();
						if (component.CanBePickup)
						{
							inventory[GetEmptyCell].SetItem(component.idItem, component.Health);
							if ((bool)_takeSound)
							{
								_takeSound.Play();
							}
							log(ItemLocalization.GetItem(component.idItem).GetFromLanguage(GlobalLocalization.currentLanguage));
							Object.Destroy(component.gameObject);
						}
					}
				}
				else if ((bool)GetUnderTap().GetComponent<ChestScript>())
				{
					ChestScript component2 = GetUnderTap().GetComponent<ChestScript>();
					if (!component2.isUnlocked)
					{
						if (inventory[GetSelectedCell].idItem == component2.IDKey)
						{
							inventory[GetSelectedCell].Clear();
							component2.Unlock();
						}
					}
					else
					{
						component2.ChangeActive();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<ControllPanel>())
				{
					ControllPanel component3 = GetUnderTap().GetComponent<ControllPanel>();
					if (!component3.active)
					{
						component3.SetActivePanel();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<BreakableWall>())
				{
					BreakableWall component4 = GetUnderTap().GetComponent<BreakableWall>();
					if (inventory[GetSelectedCell].idItem == component4.NeedItemID)
					{
						inventory[GetSelectedCell].Clear();
						component4.Break();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<FaucetObject>())
				{
					FaucetObject component5 = GetUnderTap().GetComponent<FaucetObject>();
					component5.Activate();
				}
				else if ((bool)GetUnderTap().GetComponent<ChainController>())
				{
					ChainController component6 = GetUnderTap().GetComponent<ChainController>();
					if (component6.needItem == 0)
					{
						component6.Activate();
					}
					else if (inventory[GetSelectedCell].idItem == component6.needItem)
					{
						inventory[GetSelectedCell].Clear();
						component6.Activate();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<SinkController>())
				{
					SinkController component7 = GetUnderTap().GetComponent<SinkController>();
					if (component7.screw == null)
					{
						component7.Shift();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<IsolationController>())
				{
					IsolationController component8 = GetUnderTap().GetComponent<IsolationController>();
					if (component8.NeedItem == 0)
					{
						component8.Use();
					}
					else if (inventory[GetSelectedCell].idItem == component8.NeedItem)
					{
						inventory[GetSelectedCell].Clear();
						component8.Use();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<ControllPowerButton>())
				{
					ControllPowerButton component9 = GetUnderTap().GetComponent<ControllPowerButton>();
					component9.Click();
				}
				else if ((bool)GetUnderTap().GetComponent<BoxController>())
				{
					BoxController component10 = GetUnderTap().GetComponent<BoxController>();
					component10.ChangeDirection();
				}
				else if (GetUnderTap().transform.parent != null && (bool)GetUnderTap().transform.parent.GetComponent<LatticeController>())
				{
					LatticeController component11 = GetUnderTap().transform.parent.GetComponent<LatticeController>();
					if ((bool)_bot && !_bot.InRoom)
					{
						if (component11.IsUnlocked)
						{
							component11.VoltageDead();
							component11.ChangeState();
						}
						else if (component11.IDNeedItem == 0)
						{
							component11.Unlock();
						}
						else if (inventory[GetSelectedCell].idItem == component11.IDNeedItem)
						{
							inventory[GetSelectedCell].Clear();
							component11.Unlock();
						}
					}
				}
				else if (GetUnderTap().transform.parent != null && (bool)GetUnderTap().transform.parent.GetComponent<Switcher220V>())
				{
					Switcher220V component12 = GetUnderTap().transform.parent.GetComponent<Switcher220V>();
					component12.Switch();
				}
				else if ((bool)GetUnderTap().GetComponent<ScrewController>())
				{
					ScrewController component13 = GetUnderTap().GetComponent<ScrewController>();
					if (inventory[GetSelectedCell].idItem == component13.needItem)
					{
						component13.Unscrew();
						inventory[GetSelectedCell].Damage();
					}
					else if (component13.needItem == 0)
					{
						component13.Unscrew();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<StapleController>())
				{
					StapleController component14 = GetUnderTap().GetComponent<StapleController>();
					if (inventory[GetSelectedCell].idItem == component14.needItem)
					{
						component14.Unscrew();
						inventory[GetSelectedCell].Damage();
					}
					else if (component14.needItem == 0)
					{
						component14.Unscrew();
					}
				}
				else if ((bool)GetUnderTap().transform.parent && (bool)GetUnderTap().transform.parent.GetComponent<ToiletController>())
				{
					ToiletController component15 = GetUnderTap().transform.parent.GetComponent<ToiletController>();
					component15.Open();
				}
				else if ((bool)GetUnderTap().GetComponent<ViseController>())
				{
					ViseController component16 = GetUnderTap().GetComponent<ViseController>();
					if (!component16.CanBeUse)
					{
						if (inventory[GetSelectedCell].idItem == component16.NeedItem)
						{
							component16.SetItem();
							inventory[GetSelectedCell].Damage();
						}
					}
					else
					{
						component16.Use();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<CraftBoardController>())
				{
					CraftBoardController component17 = GetUnderTap().GetComponent<CraftBoardController>();
					if (component17.NeedItem(inventory[GetSelectedCell].idItem))
					{
						component17.Craft(inventory[GetSelectedCell].idItem);
						inventory[GetSelectedCell].Clear();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<BarController>())
				{
					BarController component18 = GetUnderTap().GetComponent<BarController>();
					if (inventory[GetSelectedCell].idItem == component18.needItem)
					{
						component18.Active();
						inventory[GetSelectedCell].Damage();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<ElevatorMechanism>())
				{
					ElevatorMechanism component19 = GetUnderTap().GetComponent<ElevatorMechanism>();
					if (!component19.CanBeUse)
					{
						if (inventory[GetSelectedCell].idItem == component19.needItem)
						{
							component19.SetItem();
							inventory[GetSelectedCell].Damage();
						}
					}
					else
					{
						component19.Use();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<SmallDoorController>())
				{
					SmallDoorController component20 = GetUnderTap().GetComponent<SmallDoorController>();
					component20.Open();
				}
				else if ((bool)GetUnderTap().GetComponent<GlassController>())
				{
					GlassController component21 = GetUnderTap().GetComponent<GlassController>();
					if (inventory[GetSelectedCell].idItem == component21.NeedItem)
					{
						component21.Destroy();
						inventory[GetSelectedCell].Damage();
					}
				}
				else if ((bool)GetUnderTap().GetComponent<MessageController>())
				{
					MessageController component22 = GetUnderTap().GetComponent<MessageController>();
					component22.Use();
				}
				else if ((bool)GetUnderTap().GetComponent<DaybookController>())
				{
					DaybookController component23 = GetUnderTap().GetComponent<DaybookController>();
					if (!component23.ui.activeInHierarchy)
					{
						component23.Use();
					}
				}
			}
			if (Input.GetMouseButtonUp(0))
			{
				timeTap = 0f;
				LockTap = false;
			}
			currentFrame++;
			if ((float)currentFrame >= 15f)
			{
				currentFrame = 0;
				for (byte b = 0; b < inventory.Length; b++)
				{
					inventory[b].SetSelected(b == GetSelectedCell, _selectedStorkeColor);
				}
			}
		}

		public sbyte FindItem(byte id)
		{
			for (sbyte b = 0; b < inventory.Length; b++)
			{
				if (inventory[b].idItem == id)
				{
					return b;
				}
			}
			return -1;
		}

		public void SetSelectedCell(int id)
		{
			GetSelectedCell = (byte)id;
		}
	}
}
