using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UObject = UnityEngine.Object;
using SObject = System.Object;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace AillieoUtils
{

    public static class Utils
    {
        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static bool DeserializeBytesToData<T>(string filename, out T obj)
        {
            if (!Utils.FileExists(filename))
            {
                obj = default(T);
                return false;
            }
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                IFormatter formatter = new BinaryFormatter();
                obj = (T)formatter.Deserialize(stream);
                stream.Close();
                return true;
            }
        }


        public static bool SerializeDataToBytes<T>(T obj, string filename)
        {
            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Close();
                return true;
            }
        }
        public static Texture2D Base64ToTexture2D(string base64)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.LoadImage(Convert.FromBase64String(base64));
            tex.Apply();
            return tex;
        }


    }

}
