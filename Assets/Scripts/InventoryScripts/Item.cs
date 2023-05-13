using Player;
using UnityEngine;

namespace InventoryScripts
{
    [CreateAssetMenu(menuName = "Scriptable object/Item")]
    public class Item : ScriptableObject
    {
        [Header("Only gameplay")] public ItemType type;
        public TierLevel tierLevel;
        public ActionType actionType;
        [Header("Only UI")] public bool stackable = true;

        [Header("Both")] public Sprite image;
        [SerializeField] public int maxStackSize;
        [SerializeField] public string itemName, itemDescription;

        public void UseHealItem(int itemTier)
        {
            if (type == ItemType.root)
            {
                if (PlayerHealthBehaviour.instance.currentHealth < PlayerHealthBehaviour.instance.maxHealth)
                {
                    switch (itemTier)
                    {
                        case 1:
                            PlayerHealthBehaviour.instance.currentHealth++;
                            break;
                        case 2:
                            PlayerHealthBehaviour.instance.currentHealth += 2;
                            break;
                        case 3:
                            PlayerHealthBehaviour.instance.currentHealth += 3;
                            break;
                    }
                }

                InventoryManager.instance.GetSelectedItem(true);
            }
        }
    }

    public enum ItemType
    {
        root,
        tool
    }

    public enum ActionType
    {
        eat,
        drop,
        harvest,
        destroy
    }

    public enum TierLevel
    {
        one,
        two,
        three
    }
}