using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneRenderTxture : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    
    public Texture2D GrabPass(Camera c1,Camera c2)
    {
        camera1 = c1;
        camera2 = c2;
        return CaptureCamera(Screen.width,Screen.height);
    }
    
    /// <summary>  
    /// 对相机截图。   
    /// </summary>  
    /// <returns>The screenshot2.</returns>  
    /// <param name="camera">Camera.要被截屏的相机</param>  
    /// <param name="rect">Rect.截屏的区域</param>  
    Texture2D CaptureCamera(int width,int height)   
    {  
        Rect rect = new Rect(0,0,width,height);
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);  
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机  
        camera2.targetTexture = rt;  
        camera2.Render();  
        camera1.targetTexture = rt;  
        camera1.Render();  
       
  
        RenderTexture.active = rt;  
        Texture2D texture2D = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24,false);  
        texture2D.ReadPixels(rect, 0, 0);// 注：这个时候，它是从RenderTexture.active中读取像素  
        texture2D.Apply();  
  
        // 重置相关参数  
        camera1.targetTexture = null;  
        camera2.targetTexture = null;  
        RenderTexture.active = null;  
        Destroy(rt);  
        
        return texture2D;  
    } 
}
