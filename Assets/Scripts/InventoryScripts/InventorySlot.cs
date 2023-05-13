using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryScripts
{
    public class InventorySlot : MonoBehaviour, IDropHandler, IPointerClickHandler
    {
        public Image image;
        public Color selectedColor, notSelectedColor;
        public InventoryManager inventoryManager;

        private void Awake()
        {
            Deselect();
        }

        public void Select()
        {
            image.color = selectedColor;
        }

        public void Deselect()
        {
            image.color = notSelectedColor;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount == 0)
            {
                InventoryItems inventoryItem = eventData.pointerDrag.GetComponent<InventoryItems>();
                inventoryItem.parentAfterDrag = transform;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            var selectedSlot = Array.IndexOf(inventoryManager.inventorySlots, this);
            Debug.Log(selectedSlot);
            inventoryManager.ChangeSelectedSlot(selectedSlot,inventoryManager._selectedSlot);
        }
    }
}