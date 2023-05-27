using UnityEngine;
using UnityEngine.UI;

public class BuildingDeskPanelController : MonoBehaviour
{
    [SerializeField] private BuildingItem _buildingItem;
    [SerializeField] private Text itemName;
    [SerializeField] private BuildingController _buildingController;
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private BalanceBehaviour _balanceBehaviour;
    private BuildingDeskController[] _buildingDeskControllers;

    private void Start()
    {
        itemName = GetComponent<Text>();
        itemName.text = _buildingItem.itemName + " " + _buildingItem.cost;
        _balanceBehaviour = FindObjectOfType<BalanceBehaviour>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            buildingPanel.SetActive(false);
            _buildingDeskControllers = FindObjectsOfType<BuildingDeskController>();
            foreach (var t in _buildingDeskControllers)
            {
                if (t._opened)
                {
                    t.CloseBuildingDesk();
                }
            }
        }
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