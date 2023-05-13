using System;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace InventoryScripts
{
    public class InventoryManager : MonoBehaviour
    {
        public Item[] startItems;
        public InventorySlot[] inventorySlots;
        public GameObject inventoryItemPrefab;

        [HideInInspector] [SerializeField] public GameObject mainInventory;
        [HideInInspector] public bool _opened;
        private ChestController[] _chestController;

        public static InventoryManager instance;

        public int _selectedSlot;
        public Tilemap map;
        public Transform player;
        
        private void Awake()
        {
            instance = this;
            _opened = false;
            _chestController = FindObjectsOfType<ChestController>();
        }

        private void Start()
        {
            ChangeSelectedSlot(0, 0);
            foreach (var item in startItems)
                AddItem(item);
        }

        private void Update()
        {
            if (Input.inputString != null)
            {
                bool isNumber = int.TryParse(Input.inputString, out int number);
                if (isNumber && number is > 0 and < 11)
                {
                    ChangeSelectedSlot(number - 1, _selectedSlot);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (_opened == false)
                {
                    _opened = true;
                    mainInventory.SetActive(true);
                }
                else
                {
                    _opened = false;
                    mainInventory.gameObject.SetActive(false);
                    for (int i = 0; i < _chestController.Length; i++)
                    {
                        if (_chestController[i]._opened)
                        {
                            _chestController[i].CloseChest();
                        }
                    }
                }
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int position = map.WorldToCell(mousePosition);

            if (Input.GetMouseButtonDown(1))
            {
                InventorySlot slot = inventorySlots[_selectedSlot];
                InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
                if (itemInSlot.item.actionType == ActionType.eat)
                {
                    switch (itemInSlot.item.tierLevel)
                    {
                        case TierLevel.one:
                            itemInSlot.item.UseHealItem(1);
                            break;
                        case TierLevel.two:
                            itemInSlot.item.UseHealItem(2);
                            break;

                        case TierLevel.three:
                            itemInSlot.item.UseHealItem(3);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (itemInSlot.item.actionType == ActionType.harvest)
                {
                    if (GameManager.instance.tileManager.IsInteractable(position))
                    {
                        if (Mathf.Abs(Vector3.Distance(player.position, mousePosition)) <= 1f)
                        {
                            GameManager.instance.tileManager.SetInteracted(position);
                        }
                    }
                }

                if (itemInSlot.item.actionType == ActionType.destroy)
                {
                    if (Mathf.Abs(Vector3.Distance(player.position, mousePosition)) <= 1f)
                    {
                        GameManager.instance.tileManager.BreakTile(position);
                    }
                }
            }
        }

        public void ChangeSelectedSlot(int newValue, int selectedSlot)
        {
            inventorySlots[selectedSlot].Deselect();
            inventorySlots[newValue].Select();
            _selectedSlot = newValue;
        }

        public bool AddItem(Item item)
        {
            //Check if any slot has the same item with count lower than max stack size
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
                if (itemInSlot != null && itemInSlot.item == item &&
                    itemInSlot.count < itemInSlot.item.maxStackSize &&
                    itemInSlot.item.stackable)
                {
                    itemInSlot.count++;
                    itemInSlot.RefreshCount();
                    return true;
                }
            }

            //Find any empty slot
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                InventorySlot slot = inventorySlots[i];
                InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
                if (itemInSlot == null)
                {
                    SpawnNewItem(item, slot);
                    return true;
                }
            }

            return false;
        }

        void SpawnNewItem(Item item, InventorySlot slot)
        {
            GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            InventoryItems inventoryItem = newItemGO.GetComponent<InventoryItems>();
            inventoryItem.InitialiseItem(item);
        }

        public Item GetSelectedItem(bool use)
        {
            InventorySlot slot = inventorySlots[_selectedSlot];
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot != null)
            {
                Item item = itemInSlot.item;
                if (use && _opened == false)
                {
                    itemInSlot.count--;
                    if (itemInSlot.count <= 0)
                    {
                        Destroy(itemInSlot.gameObject);
                    }
                    else
                    {
                        itemInSlot.RefreshCount();
                    }
                }

                return item;
            }

            return null;
        }
    }
}