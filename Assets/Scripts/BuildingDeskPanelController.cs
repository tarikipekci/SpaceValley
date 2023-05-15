using UnityEngine;
using UnityEngine.UI;

public class BuildingDeskPanelController : MonoBehaviour
{
    [SerializeField] private BuildingItem _buildingItem;
    [SerializeField] private Text itemName;
    [SerializeField] private BuildingController _buildingController;

    private void Start()
    {
        itemName = GetComponent<Text>();
        itemName.text = _buildingItem.itemName + " " + _buildingItem.cost;
    }


    public void OnClick()
    {
        Debug.Log(_buildingItem.name);
        BuildingController.instance.builderMode = true;
        _buildingController.selectedConstruction = _buildingItem.buildingItem;
    }
}