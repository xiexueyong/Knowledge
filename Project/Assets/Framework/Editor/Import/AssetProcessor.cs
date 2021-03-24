// namespace MagicTavern.March.Editor.Import
// {
//     using UnityEngine;
//     using UnityEditor;
//
//     public class MarchAssetProcessor : AssetPostprocessor
//     {
//         private void OnPreprocessModel()
//         {
//             var importer = (ModelImporter) assetImporter;
//             MarchModelProcessor.OnPreprocess(assetPath, importer);
//         }
//
//         private void OnPostprocessModel(GameObject model)
//         {
//             MarchModelProcessor.OnPostprocess(assetPath, model);
//         }
//
//         private void OnPreprocessAudio()
//         {
//             var importer = (AudioImporter) assetImporter;
//             MarchAudioProcessor.OnPreprocess(assetPath, importer);
//         }
//
//         private void OnPostprocessAudio(AudioClip audio)
//         {
//             MarchAudioProcessor.OnPostprocess(assetPath, audio);
//         }
//
//         private void OnPreprocessTexture()
//         {
//             var importer = (TextureImporter) assetImporter;
//             MarchTextureProcessor.OnPreprocess(assetPath, importer);
//         }
//
//         private void OnPostprocessTexture(Texture2D texture)
//         {
//             MarchTextureProcessor.OnPostprocess(assetPath, texture);
//         }
//
//         private void OnPreprocessAnimation()
//         {
//             var importer = (ModelImporter) assetImporter;
//             MarchAnimationProcessor.OnPreprocess(assetPath, importer);
//         }
//     }
// }