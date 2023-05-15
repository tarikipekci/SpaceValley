using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Building Item")]
public class BuildingItem : ScriptableObject
{
    [SerializeField] private GameObject buildingItem;
    [SerializeField] public string itemName, itemDescription;
    public Sprite image;
    [SerializeField] public int cost;
}
