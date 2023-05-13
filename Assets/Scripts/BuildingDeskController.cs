using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDeskController : MonoBehaviour
{
   private Animator _buildingDeskAnim;
    private static readonly int Opened = Animator.StringToHash("opened");
    public bool _opened;
    private static readonly int Closed = Animator.StringToHash("closed");
    private static readonly int Outline = Animator.StringToHash("outline");

    private void OnMouseDown()
    {
        BuildingController.instance.builderMode = true;
    }
}
