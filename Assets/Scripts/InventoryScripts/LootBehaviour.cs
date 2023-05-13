using UnityEngine;

namespace InventoryScripts
{
    public class LootBehaviour : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Item item;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spriteRenderer.sprite = item.image;
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                bool canAdd = InventoryManager.instance.AddItem(item);
                if (canAdd)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}