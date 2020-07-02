using System.Collections.Generic;
using System.Linq;
using UnityEditor.U2D;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace UnityEditor.U2D.PSD
{
    internal class PSDImportPostProcessor : AssetPostprocessor
    {
        void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
        {
            var dataProviderFactories = new SpriteDataProviderFactories();
            dataProviderFactories.Init();
            ISpriteEditorDataProvider importer = dataProviderFactories.GetSpriteEditorDataProviderFromObject(AssetImporter.GetAtPath(assetPath));
            if (importer != null)
            {
                importer.InitSpriteEditorDataProvider();
                var physicsOutlineDataProvider = importer.GetDataProvider<ISpritePhysicsOutlineDataProvider>();
                var textureDataProvider = importer.GetDataProvider<ITextureDataProvider>();
                int actualWidth = 0, actualHeight = 0;
                textureDataProvider.GetTextureActualWidthAndHeight(out actualWidth, out actualHeight);
                float definitionScaleW = (float)texture.width / actualWidth;
                float definitionScaleH = (float)texture.height / actualHeight;
                float definitionScale = Mathf.Min(definitionScaleW, definitionScaleH);
                foreach (var sprite in sprites)
                {
                    var guid = sprite.GetSpriteID();
                    var outline = physicsOutlineDataProvider.GetOutlines(guid);
                    var outlineOffset = sprite.rect.size / 2;
                    if (outline != null && outline.Count > 0)
                    {
                        var convertedOutline = new Vector2[outline.Count][];
                        for (int i = 0; i < outline.Count; ++i)
                        {
                            convertedOutline[i] = new Vector2[outline[i].Length];
                            for (int j = 0; j < outline[i].Length; ++j)
                            {
                                convertedOutline[i][j] = outline[i][j] * definitionScale + outlineOffset;
                            }
                        }
                        sprite.OverridePhysicsShape(convertedOutline);
                    }
                }
            }
        }
    }
}
