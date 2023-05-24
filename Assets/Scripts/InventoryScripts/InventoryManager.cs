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
                    _chestController = FindObjectsOfType<ChestController>();
                    foreach (var t in _chestController)
                    {
                        if (t._opened)
                        {
                            t.CloseChest();
                        }
                    }
                }
            }

            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int position = map.WorldToCell(mousePosition);

            if (!Input.GetMouseButtonDown(1)) return;
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

            switch (itemInSlot.item.actionType)
            {
                case ActionType.harvest:
                {
                    if (GameManager.instance.tileManager.IsInteractable(position))
                    {
                        if (Mathf.Abs(Vector3.Distance(player.position, mousePosition)) <= 1f)
                        {
                            GameManager.instance.tileManager.SetInteracted(position);
                        }
                    }

                    break;
                }
                case ActionType.destroy:
                {
                    if (Mathf.Abs(Vector3.Distance(player.position, mousePosition)) <= 1f)
                    {
                        GameManager.instance.tileManager.BreakTile(position);
                    }

                    break;
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
            foreach (var slot in inventorySlots)
            {
                var itemInSlot = slot.GetComponentInChildren<InventoryItems>();
                if (itemInSlot == null || itemInSlot.item != item ||
                    itemInSlot.count >= itemInSlot.item.maxStackSize ||
                    !itemInSlot.item.stackable) continue;
                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }

            //Find any empty slot
            foreach (var slot in inventorySlots)
            {
                var itemInSlot = slot.GetComponentInChildren<InventoryItems>();
                if (itemInSlot != null) continue;
                SpawnNewItem(item, slot);
                return true;
            }

            return false;
        }

        void SpawnNewItem(Item item, InventorySlot slot)
        {
            var newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
            var inventoryItem = newItemGO.GetComponent<InventoryItems>();
            inventoryItem.InitialiseItem(item);
        }

        public Item GetSelectedItem(bool use)
        {
            var slot = inventorySlots[_selectedSlot];
            var itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot == null) return null;
            var item = itemInSlot.item;
            if (!use || _opened) return item;
            itemInSlot.count--;
            if (itemInSlot.count <= 0)
            {
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                itemInSlot.RefreshCount();
            }

            return item;

        }
    }
}