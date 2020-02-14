using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteBouncer : MonoBehaviour
{
    public string sheetName;
    private void Start()
    {
        BounceSpriteSheet();
        //PrintPath();
    }

    public void BounceSpriteSheet()
    {
        Sprite[] sheet = Resources.LoadAll<Sprite>(sheetName);

        foreach (Sprite sprite in sheet)
        {
            Texture2D tex = sprite.texture;
            Rect r = sprite.textureRect;
            Texture2D subtex = tex.CropTexture((int)r.x, (int)r.y, (int)r.width, (int)r.height);
            byte[] data = subtex.EncodeToPNG();
            File.WriteAllBytes(Application.persistentDataPath + "/" + sprite.name + ".png", data);

            Debug.Log("SprintBouncer exported sprite sheet to "+ Application.persistentDataPath);
        }
    }
    public void PrintPath()
    {
        Debug.Log(Application.persistentDataPath);
    }
}
