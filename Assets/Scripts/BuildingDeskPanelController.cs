using UnityEngine;
using UnityEngine.UI;

public class BuildingDeskPanelController : MonoBehaviour
{
    [SerializeField] private BuildingItem _buildingItem;
    [SerializeField] private Text itemName;
    [SerializeField] private BuildingController _buildingController;
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private BalanceBehaviour _balanceBehaviour;

    private void Start()
    {
        itemName = GetComponent<Text>();
        itemName.text = _buildingItem.itemName + " " + _buildingItem.cost;
        _balanceBehaviour = FindObjectOfType<BalanceBehaviour>();
    }


    public void OnClick()
    {
        if (_balanceBehaviour.balance >= _buildingItem.cost)
        {
            BuildingController.instance.builderMode = true;
            _buildingController.selectedConstruction = _buildingItem.buildingItem;
            buildingPanel.SetActive(false);
            _balanceBehaviour.SpendMoney(_buildingItem.cost);
        }
        else
        {
           WarningMessageController.instance.PrintMessage("Insufficient balance");
        }
    }
}