using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
   public TileManager tileManager;
   public static GameManager instance;

   private void Awake()
   {
      if (instance != null && instance != this)
      {
         Destroy(this.gameObject);
      }
      else
      {
         instance = this;
      }
      DontDestroyOnLoad(this.gameObject);
      tileManager = GetComponent<TileManager>();
   }
}
