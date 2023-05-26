using UnityEngine;

public class PropBehaviour : MonoBehaviour
{
    [SerializeField] private BuildingController _buildingController;

    private void Awake()
    {
        _buildingController = BuildingController.instance;
        _buildingController._canBuild = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _buildingController._spawnItem)
        {
            _buildingController._canBuild = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == _buildingController._spawnItem)
        {
            _buildingController._canBuild = true;
        }
    }
}
