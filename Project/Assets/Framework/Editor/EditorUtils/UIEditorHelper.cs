
using UnityEditor;
using UnityEngine;



public static class UIEditorHelper
{
    //[MenuItem("Edit/Copy Names " + "%#c", false, 2)]
    [MenuItem("GameObject/Copy Path", priority = 49)]
    public static void CopySelectWidgetName()
    {
        string result = "";
        foreach (var item in Selection.gameObjects)
        {
            result = item.transform.name;
            Transform root_trans = item.transform.parent;
            while (root_trans != null)
            {
                result = root_trans.name + "/" + result;
                if (root_trans.name == "root")
                {
                    break;
                }
                root_trans = root_trans.parent;
              
            }
        }

        //复制到系统全局的粘贴板上
        result = result.Replace("\"", "");
        GUIUtility.systemCopyBuffer = result;
        Debug.Log(result);
    }

}