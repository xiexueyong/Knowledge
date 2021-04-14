using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeComponent : MonoBehaviour
{
   
   public float second = 1.5f;
   private float _passedSecond;
   private bool _show;
   public void Show()
   {
      _show = true;
      _passedSecond = second;
      if (!gameObject.activeSelf)
      {
         gameObject.SetActive(true);
      }
   }

   public void Hide()
   {
      _show = false;
      if (gameObject.activeSelf)
      {
         gameObject.SetActive(false);
      }
   }

   private void Update()
   {
      if (_show)
      {
         _passedSecond -= Time.deltaTime;
         if (_passedSecond <= 0)
         {
            Hide();
         }

      }
      
      
   }
}
