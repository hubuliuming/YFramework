using UnityEditor;

public class TextureSetting : AssetPostprocessor
{
    private void OnPreprocessTexture()
    {
        TextureImporter importer = assetImporter as TextureImporter;
        importer.textureType = TextureImporterType.Sprite;
    }
}
