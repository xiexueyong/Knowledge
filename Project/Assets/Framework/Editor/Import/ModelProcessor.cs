namespace MagicTavern.March.Editor.Import
{
    using UnityEngine;
    using UnityEditor;
    using System.Collections;

    public static class MarchModelProcessor
    {
        public static void OnPostprocess(string assetPath, GameObject model)
        {
            Debug.Log("Postprocess Model: " + assetPath);
        }

        public static void OnPreprocess(string assetPath, ModelImporter importer)
        {
            Debug.Log("Preprocess Model: " + assetPath);
        }
    }
}