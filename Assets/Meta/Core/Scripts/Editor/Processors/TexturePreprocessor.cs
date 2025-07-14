using UnityEditor;

namespace Core.Editor
{
    public class TexturePreprocessor : AssetPostprocessor
    {
        private const string AndroidPlatformName = "Android";
        private const string IPhonePlatformName = "iPhone";
        
        private void OnPreprocessTexture()
        {
            TextureImporter importer = (TextureImporter)assetImporter;

            if (importer.assetPath.Contains("Art/Sprites"))
            {
                if (importer.assetPath.Contains("IgnorePostprocess"))
                {
                    return;
                }

                if (importer.assetPath.Contains("Sprites"))
                {
                    if (importer.textureType != TextureImporterType.Sprite)
                    {
                        importer.textureType = TextureImporterType.Sprite;
                        importer.spriteImportMode = SpriteImportMode.Single;

                        importer.mipmapEnabled = false;
                    }
                }

                SetupPlatformSettings(importer, AndroidPlatformName, TextureImporterFormat.ASTC_6x6);
                SetupPlatformSettings(importer, IPhonePlatformName, TextureImporterFormat.ASTC_6x6);
            }
            else if (importer.assetPath.Contains("Art/Textures/Compressed textures"))
            {
                if (importer.assetPath.Contains("10x10"))
                {
                    SetupPlatformSettings(importer, AndroidPlatformName, TextureImporterFormat.ASTC_10x10);
                    SetupPlatformSettings(importer, IPhonePlatformName, TextureImporterFormat.ASTC_10x10);
                }
                else if (importer.assetPath.Contains("12x12"))
                {
                    SetupPlatformSettings(importer, AndroidPlatformName, TextureImporterFormat.ASTC_12x12);
                    SetupPlatformSettings(importer, IPhonePlatformName, TextureImporterFormat.ASTC_12x12);
                }
            }
        }

        private void SetupPlatformSettings(TextureImporter importer, string platformName,
            TextureImporterFormat importerFormat)
        {
            var settings = importer.GetPlatformTextureSettings(platformName);
            var defaultSettings = importer.GetDefaultPlatformTextureSettings();

            if (settings == null)
            {
                settings = new TextureImporterPlatformSettings()
                {
                    maxTextureSize = defaultSettings.maxTextureSize,
                    resizeAlgorithm = defaultSettings.resizeAlgorithm,
                };
            }

            settings.name = platformName;
            settings.overridden = true;
            settings.compressionQuality = (int)TextureCompressionQuality.Normal;
            settings.format = importerFormat;

            importer.SetPlatformTextureSettings(settings);
        }
    }
}