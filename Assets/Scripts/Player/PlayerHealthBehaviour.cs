using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerHealthBehaviour : MonoBehaviour
    {
        [SerializeField] public int currentHealth, maxHealth;
        [SerializeField] private Sprite fill;
        public Image[] healthBar;
        public static PlayerHealthBehaviour instance;

        private void Awake()
        {
            currentHealth = maxHealth;
            instance = this;
            
            if (currentHealth == maxHealth)
            {
                foreach (var t in healthBar)
                {
                    t.sprite = fill;
                }
            }
        }

        private void Update()
        {
            FillTheHealthBar();
        }

        private void FillTheHealthBar()
        {
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            
            for (int i = 0; i < currentHealth; i++)
            {
                healthBar[i].GetComponent<Image>().enabled = true;
                for (int j = currentHealth; j < healthBar.Length; j++)
                {
                    healthBar[j].GetComponent<Image>().enabled = false;
                }
            }
        }
    }
}