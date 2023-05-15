using System;
using UnityEngine;

public class BuildingDeskController : MonoBehaviour
{
    private Animator _buildingDeskAnim;
    private static readonly int Opened = Animator.StringToHash("opened");
    public bool _opened;
    private static readonly int Closed = Animator.StringToHash("closed");
    private static readonly int Outline = Animator.StringToHash("outline");
    public Transform player;
    private float distance;
    [SerializeField] private float maxRange;
    public GameObject buildingPanel;

    private void Awake()
    {
       buildingPanel.SetActive(false);
    }

    private void Start()
    {
        _buildingDeskAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        distance = Math.Abs(Vector2.Distance(player.position, transform.position));
    }

    private void OnMouseDown()
    {
        if (distance <= maxRange)
        {
            _opened = true;
            _buildingDeskAnim.SetBool(Opened, _opened);
            buildingPanel.SetActive(true);
        }
    }

    private void OnMouseOver()
    {
        if (distance <= maxRange)
        {
            _buildingDeskAnim.SetBool(Outline, true);
        }
    }

    public void OnMouseExit()
    {
        _buildingDeskAnim.SetBool(Outline, false);
    }

    public void CloseBuildingDesk()
    {
        _opened = false;
        _buildingDeskAnim.SetTrigger(Closed);
        _buildingDeskAnim.SetBool(Opened, false);
        buildingPanel.SetActive(false);
    }
}