using System;
using System.IO;
using UnityEngine;

namespace AillieoUtils
{

    public class ImageUtils
    {

        static byte[] ImageToBytes(string imageFileName)
        {
            return File.ReadAllBytes(imageFileName);
        }

        public static string ImageToBase64(string imageFileName)
        {
            byte[] buffers = ImageToBytes(imageFileName);
            return Convert.ToBase64String(buffers);
        }

        public static void Base64ToDiskImage(string base64, string filename)
        {
            byte[] buffers = Convert.FromBase64String(base64);
            File.WriteAllBytes(Application.dataPath + "/" + filename + ".png", buffers);
        }

        public static Texture2D Base64ToTexture2D(string base64)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(Convert.FromBase64String(base64));
            tex.Apply();
            return tex;
        }


        public static Sprite Base64ToSprite(string base64)
        {
            Texture2D tex = Base64ToTexture2D(base64);
            return Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), 0.5f * Vector2.one);
        }

    }

}