using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using UnityEngine;

public class ScapeTool : S_Destroy_MonoSingleton<ScapeTool>
{
   public Transform root;
   public Camera scapeCamera;

   private string scapeName;

   private GameObject scapeObj;
   //Prefab/UI/Scapes/
   public void SetScape(string scapeName)
   {
      if (this.scapeName == scapeName)
      {
         return;
      }

      if (scapeObj != null)
      {
         Res.Recycle(scapeObj);
      }
      var path = "Prefab/scapes/"+scapeName;
      scapeObj = Res.LoadResource<GameObject>(path);
      scapeObj.transform.SetParent(root);
      scapeObj.transform.localPosition = Vector3.zero;
      
      // var size2 = g.transform.Find("bg").GetComponent<SpriteRenderer>().size;
      var TextureSize = scapeObj.transform.Find("bg").GetComponent<SpriteRenderer>().bounds.size;
      float screenR = Screen.width*1f/Screen.height;
      float textureR = TextureSize.x / TextureSize.y;
      float cameraSize = TextureSize.y / 2f;
      
      scapeCamera.orthographicSize = textureR < screenR ? cameraSize*textureR/screenR : cameraSize;
   }
   
}
