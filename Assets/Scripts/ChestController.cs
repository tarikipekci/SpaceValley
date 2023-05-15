using System;
using InventoryScripts;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private Animator _chestAnim;
    private static readonly int Opened = Animator.StringToHash("opened");
    public bool _opened;
    private static readonly int Closed = Animator.StringToHash("closed");
    private static readonly int Outline = Animator.StringToHash("outline");
    [SerializeField] private GameObject chestInventory;
    public GameObject mainInventory;
    public InventoryManager inventoryManager;
    public Transform player;
    private float distance;
    [SerializeField] private float maxRange;

    private void Awake()
    {
        _chestAnim = GetComponent<Animator>();
        chestInventory.SetActive(false);
    }

    private void Update()
    {
        distance = Math.Abs(Vector3.Distance(player.position, transform.position));
    }

    private void OnMouseOver()
    {
        if (distance <= maxRange)
        {
             _chestAnim.SetBool(Outline,true);
        }
    }

    public void OnMouseExit()
    {
       _chestAnim.SetBool(Outline,false);
    }

    public void OnMouseDown()
    {
        if (_opened == false && distance <= maxRange)
        {
            _opened = true;
            _chestAnim.SetTrigger(Opened);
            chestInventory.SetActive(true);
            chestInventory.transform.SetParent(mainInventory.transform);
            mainInventory.gameObject.SetActive(true);
            inventoryManager._opened = true;
        }
    }

    public void CloseChest()
    {
        _opened = false;
        _chestAnim.SetTrigger(Closed);
        chestInventory.SetActive(false);
        chestInventory.transform.SetParent(this.gameObject.transform);
    }
}
