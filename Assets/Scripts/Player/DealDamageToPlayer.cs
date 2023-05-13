using Player;
using UnityEngine;

public class DealDamageToPlayer : MonoBehaviour
{
   private PlayerHealthBehaviour _playerHealthBehaviour;

   private void Awake()
   {
      _playerHealthBehaviour = FindObjectOfType<PlayerHealthBehaviour>();
   }
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         _playerHealthBehaviour.currentHealth--;
      }
   }
}
