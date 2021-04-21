using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using UnityEngine;

public class ScapeTool : S_Destroy_MonoSingleton<ScapeTool>
{
   public Transform root;
   public Camera scapeCamera;

   private string sceneName;
   //Prefab/UI/Scapes/
   public void SetScape(string scapeName)
   {
      if (this.sceneName == scapeName)
      {
         return;
      }
      var path = "Prefab/UI/Scapes/scape1";
      GameObject g = Res.LoadResource<GameObject>(path);
      g.transform.SetParent(root);
      g.transform.localPosition = Vector3.zero;

      
      var size2 = g.transform.GetComponent<SpriteRenderer>().size;

      var TextureSize = g.transform.GetComponent<SpriteRenderer>().bounds.size;
      float screenR = Screen.width*1f/Screen.height;
      float textureR = TextureSize.x / TextureSize.y;
      
      float cameraSize = TextureSize.y / 2f;
      
      
      scapeCamera.orthographicSize = textureR < screenR ? cameraSize*textureR/screenR : cameraSize;


   }
   
}
