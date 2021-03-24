namespace MagicTavern.March.Editor.Import
{
    using UnityEngine;
    using UnityEditor;
    using System;

    public static class MarchAudioProcessor
    {
        public static void OnPostprocess(string assetPath, AudioClip audio)
        {
            Debug.Log("Postprocess Audio: " + assetPath);
        }

        public static void OnPreprocess(string assetPath, AudioImporter importer)
        {
            Debug.Log("Preprocess Audio: " + assetPath);
            if (assetPath.StartsWith("Assets/Annex/Resources/Audio/Musics", StringComparison.Ordinal))
            {
                OnPreprocessMusicClip(assetPath, importer);
            }
            if (assetPath.StartsWith("Assets/Annex/Resources/Audio/Sounds", StringComparison.Ordinal))
            {
                OnPreprocessSoundClip(assetPath, importer);
            }
        }

        static void OnPreprocessMusicClip(string assetPath, AudioImporter importer)
        {
            importer.loadInBackground = true;

            var standalone = new AudioImporterSampleSettings();
            standalone.quality = 0.7f;
            standalone.loadType = AudioClipLoadType.Streaming;
            standalone.compressionFormat = AudioCompressionFormat.Vorbis;
            standalone.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("Standalone", standalone);

            var iOS = new AudioImporterSampleSettings();
            iOS.quality = 0.7f;
            iOS.loadType = AudioClipLoadType.Streaming;
            iOS.compressionFormat = AudioCompressionFormat.MP3;
            iOS.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("iOS", iOS);

            var android = new AudioImporterSampleSettings();
            android.quality = 0.7f;
            android.loadType = AudioClipLoadType.Streaming;
            android.compressionFormat = AudioCompressionFormat.Vorbis;
            android.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("Android", android);
        }

        static void OnPreprocessSoundClip(string assetPath, AudioImporter importer)
        {
            importer.loadInBackground = false;

            var standalone = new AudioImporterSampleSettings();
            standalone.quality = 0.7f;
            standalone.loadType = AudioClipLoadType.DecompressOnLoad;
            standalone.compressionFormat = AudioCompressionFormat.PCM;
            standalone.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("Standalone", standalone);

            var iOS = new AudioImporterSampleSettings();
            iOS.quality = 0.7f;
            iOS.loadType = AudioClipLoadType.DecompressOnLoad;
            iOS.compressionFormat = AudioCompressionFormat.MP3;
            iOS.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("iOS", iOS);

            var android = new AudioImporterSampleSettings();
            android.quality = 0.7f;
            android.loadType = AudioClipLoadType.DecompressOnLoad;
            android.compressionFormat = AudioCompressionFormat.ADPCM;
            android.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
            importer.SetOverrideSampleSettings("Android", android);
        }
    }
}