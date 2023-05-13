using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private void FixedUpdate()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, 0.004f);
    }
}