namespace MagicTavern.March.Editor.Import
{
    using System;
    using UnityEngine;
    using UnityEditor;

    public static class MarchTextureProcessor
    {
        public static void OnPostprocess(string assetPath, Texture2D texture)
        {
        }

        public static void OnPreprocess(string assetPath, TextureImporter importer)
        {
            importer.maxTextureSize = 2048;
            if (assetPath.StartsWith("Assets/Art/Spine/", StringComparison.Ordinal))
            {
                OnPreprocessSpine(assetPath, importer);
            }

            if (assetPath.StartsWith("Assets/Art/Atlas/", StringComparison.Ordinal))
            {
                OnPreprocessSprite(assetPath, importer);
            }

            if (assetPath.StartsWith("Assets/Art/Model/", StringComparison.Ordinal))
            {
                OnPreProcessModelTexture(assetPath, importer);
            }
        }

        static void OnPreprocessSpine(string assetPath, TextureImporter importer)
        {
            Debug.Log("Spine Preprocess Texture: " + assetPath);
            importer.textureType = TextureImporterType.Default;
            importer.sRGBTexture = true;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.alphaSource = TextureImporterAlphaSource.None;
            importer.alphaIsTransparency = false;
            importer.npotScale = TextureImporterNPOTScale.ToNearest;
            importer.textureCompression = TextureImporterCompression.Compressed;

            var iPhone = new TextureImporterPlatformSettings();
            iPhone.name = "iPhone";
            iPhone.overridden = true;
            iPhone.format = TextureImporterFormat.ASTC_RGBA_4x4;
            iPhone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            iPhone.textureCompression = TextureImporterCompression.Compressed;
            importer.SetPlatformTextureSettings(iPhone);

            var android = new TextureImporterPlatformSettings();
            android.name = "Android";
            android.overridden = true;
            android.format = TextureImporterFormat.RGBA32;
            //android.compressionQuality = 100;
            android.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            android.textureCompression = TextureImporterCompression.Compressed;
            android.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32BitDownscaled;
            importer.SetPlatformTextureSettings(android);

            var standalone = new TextureImporterPlatformSettings();
            standalone.name = "Standalone";
            standalone.overridden = true;
            standalone.format = TextureImporterFormat.RGBA32;
            standalone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            standalone.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SetPlatformTextureSettings(standalone);
        }

        static void OnPreprocessSprite(string assetPath, TextureImporter importer)
        {
            Debug.Log("Sprite Preprocess Texture: " + assetPath);


            string[] file = assetPath.Split('/');
            if (file.Length > 1)
            {
                importer.spritePackingTag = file[file.Length - 2];
            }

 
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;
            importer.sRGBTexture = true;
            importer.isReadable = false;
            importer.mipmapEnabled = false;
            importer.wrapMode = TextureWrapMode.Clamp;
            importer.filterMode = FilterMode.Bilinear;
            importer.alphaSource = TextureImporterAlphaSource.FromInput;
            importer.alphaIsTransparency = true;
            //importer.npotScale = TextureImporterNPOTScale.ToNearest;
            importer.textureCompression = TextureImporterCompression.Compressed;

            var iPhone = new TextureImporterPlatformSettings();
            iPhone.name = "iPhone";
            iPhone.overridden = true;
            iPhone.format = TextureImporterFormat.ASTC_RGBA_4x4;
            //iPhone.format = TextureImporterFormat.PVRTC_RGBA4;
            iPhone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            iPhone.textureCompression = TextureImporterCompression.Compressed;
            importer.SetPlatformTextureSettings(iPhone);

            var android = new TextureImporterPlatformSettings();
            android.name = "Android";
            android.overridden = true;
            android.format = TextureImporterFormat.ETC2_RGBA8;
            //android.compressionQuality = 100;
            android.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            android.textureCompression = TextureImporterCompression.Compressed;
            android.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32BitDownscaled;
            importer.SetPlatformTextureSettings(android);

            var standalone = new TextureImporterPlatformSettings();
            standalone.name = "Standalone";
            standalone.overridden = true;
            standalone.format = TextureImporterFormat.RGBA32;
            standalone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            standalone.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SetPlatformTextureSettings(standalone);
        }


        static void OnPreProcessModelTexture(string assetPath, TextureImporter importer)
        {
            Debug.Log("ModelTexture Preorocess Texture: " + assetPath);

            if (assetPath.Contains("_01_d"))
            {
                importer.textureType = TextureImporterType.Default;
                importer.textureShape = TextureImporterShape.Texture2D;
                importer.sRGBTexture = true;
                importer.alphaSource = TextureImporterAlphaSource.None;
                importer.alphaIsTransparency = true;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Bilinear;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.isReadable = true;
                if (assetPath.Contains("_hd_01_d") || assetPath.Contains("_g_01_d"))
                {
                    importer.isReadable = false;
                }

                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Compressed;
            }
            else if (assetPath.Contains("_01_n"))
            {
                importer.textureType = TextureImporterType.NormalMap;
                importer.textureShape = TextureImporterShape.Texture2D;
                importer.wrapMode = TextureWrapMode.Clamp;
                importer.filterMode = FilterMode.Bilinear;
                importer.npotScale = TextureImporterNPOTScale.None;
                importer.isReadable = false;
                importer.mipmapEnabled = false;
                importer.textureCompression = TextureImporterCompression.Compressed;
            }
            else
            {
                return;
            }

            var iPhone = new TextureImporterPlatformSettings();
            iPhone.name = "iPhone";
            iPhone.overridden = true;
            iPhone.format = TextureImporterFormat.ASTC_RGBA_4x4;
            iPhone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            iPhone.textureCompression = TextureImporterCompression.Compressed;
            importer.SetPlatformTextureSettings(iPhone);

            var android = new TextureImporterPlatformSettings();
            android.name = "Android";
            android.overridden = true;
            android.format = TextureImporterFormat.ETC2_RGBA8Crunched; //ETC2_RGBA8;
            android.compressionQuality = 100;
            android.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            android.textureCompression = TextureImporterCompression.Compressed;
            android.androidETC2FallbackOverride = AndroidETC2FallbackOverride.Quality32BitDownscaled;
            importer.SetPlatformTextureSettings(android);

            var standalone = new TextureImporterPlatformSettings();
            standalone.name = "Standalone";
            standalone.overridden = true;
            standalone.format = TextureImporterFormat.RGBA32;
            standalone.resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
            standalone.textureCompression = TextureImporterCompression.Uncompressed;
            importer.SetPlatformTextureSettings(standalone);
        }
    }
}