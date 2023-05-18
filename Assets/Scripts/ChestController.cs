using System;
using InventoryScripts;
using Player;
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
    public PlayerBehaviour player;
    public float distance;
    [SerializeField] private float maxRange;
    public static ChestController instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerBehaviour>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        _chestAnim = GetComponent<Animator>();
        chestInventory.SetActive(false);
        mainInventory = inventoryManager.mainInventory;
        mainInventory.SetActive(false);
    }

    private void Update()
    {
        distance = Math.Abs(Vector3.Distance(player.transform.position, transform.position));
    }

    private void OnMouseOver()
    {
        if (distance <= maxRange)
        {
            _chestAnim.SetBool(Outline, true);
        }
    }

    public void OnMouseExit()
    {
        _chestAnim.SetBool(Outline, false);
    }

    public void OnMouseDown()
    {
        if (_opened == false && distance <= maxRange)
        {
            _opened = true;
            _chestAnim.SetTrigger(Opened);
            chestInventory.SetActive(true);
            chestInventory.transform.SetParent(mainInventory.transform);
            mainInventory.SetActive(true);
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