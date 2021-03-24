namespace MagicTavern.March.Editor.Import
{
    using UnityEngine;
    using UnityEditor;

    public static class MarchAnimationProcessor
    {
        public static void OnPreprocess(string assetPath, ModelImporter importer)
        {
            Debug.Log("Preprocess Animation: " + assetPath);
        }
    }
}