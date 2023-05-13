using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace InventoryScripts
{
    public class InventoryItems : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        [Header("UI")] public Image image;
        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public Item item;
        [HideInInspector] public int count = 1;
        public Text countText;
        [SerializeField] private GameObject itemDetailPanel;
        public Text itemName;
        public Text itemDescription;
        public Image itemImage;

        private void Start()
        {
            itemDetailPanel.SetActive(false);
            itemName.text = item.itemName;
            itemDescription.text = item.itemDescription;
            itemImage.sprite = item.image;
        }
        
        public void InitialiseItem(Item newItem)
        {
            item = newItem;
            image.sprite = newItem.image;
            RefreshCount();
        }

        public void RefreshCount()
        {
            countText.text = count.ToString();
            bool textActive = count > 1;
            countText.gameObject.SetActive(textActive);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            image.raycastTarget = false;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            itemDetailPanel.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            itemDetailPanel.SetActive(false);
        }
    }
}