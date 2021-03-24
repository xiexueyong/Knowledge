using UnityEngine;
using UnityEditor;
public class FindMissingScriptsRecursively : EditorWindow
{
    static int go_count = 0, components_count = 0, missing_count = 0;

    [MenuItem("DevHelper/FindMissingScriptsRecursively")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
    }

    public void OnGUI()
    {
        if (GUILayout.Button("Find Missing Scripts in selected GameObjects"))
        {
            FindInSelected();
        }
    }
    private static void FindInSelected()
    {
        GameObject[] go = Selection.gameObjects;
        go_count = 0;
        components_count = 0;
        missing_count = 0;
        foreach (GameObject g in go)
        {
            FindInGO(g);
        }
        Debug.Log(string.Format("Searched {0} GameObjects, {1} components, found {2} missing", go_count, components_count, missing_count));
    }

    private static void FindInGO(GameObject go)
    {
        // 创建一个序列化对象
        SerializedObject serializedObject = new SerializedObject(go);
        // 获取组件列表属性
        SerializedProperty prop = serializedObject.FindProperty("m_Component");

        var components = go.GetComponents<Component>();
        int r = 0;
        for (int j = 0; j < components.Length; j++)
        {
            // 如果组建为null
            if (components[j] == null)
            {
                // 按索引删除
                prop.DeleteArrayElementAtIndex(j - r);
                r++;
            }
        }

        
        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in go.transform)
        {
            //Debug.Log("Searching " + childT.name  + " " );
            FindInGO(childT.gameObject);
        }


        // 应用修改到对象
        serializedObject.ApplyModifiedProperties();
    }
    private static void FindInGO2(GameObject g)
    {
        go_count++;
        Component[] components = g.GetComponents<Component>();
        for (int i = 0; i < components.Length; i++)
        {
            components_count++;
            if (components[i] == null)
            {
                missing_count++;
                string s = g.name;
                Transform t = g.transform;
                while (t.parent != null)
                {
                    s = t.parent.name + "/" + s;
                    t = t.parent;
                }
                Object.DestroyImmediate(components[i]);
                
                Debug.Log(s + " has an empty script attached in position: " + i, g);
            }
        }
        // Now recurse through each child GO (if there are any):
        foreach (Transform childT in g.transform)
        {
            //Debug.Log("Searching " + childT.name  + " " );
            FindInGO(childT.gameObject);
        }
    }
}