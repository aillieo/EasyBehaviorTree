using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

namespace AillieoUtils
{
    public static class SerializeHelper
    {

        public static bool DeserializeBytesToData<T>(string filename, out T obj)
        {
            if(!FileUtils.FileExists(filename))
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


        public static byte[] SerializeDataToBytes<T>(T obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                byte[] byteArray = stream.ToArray();
                stream.Close();
                return byteArray;
            }
        }


        public static T DeserializeBytesToData<T>(byte[] byteArray)
        {
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                IFormatter formatter = new BinaryFormatter();
                T t = (T)formatter.Deserialize(stream);
                stream.Close();
                return t;
            }
        }


        public static bool DeserializeJsonToData<T>(string filename, out T obj)
        {
            if (!FileUtils.FileExists(filename))
            {
                obj = default(T);
                return false;
            }

            using (StreamReader sr = new StreamReader(filename, Encoding.UTF8))
            {
                var json = sr.ReadToEnd();
                obj = UnityEngine.JsonUtility.FromJson<T>(json);
            }
            return true;
        }


        public static bool SerializeDataToJson<T>(T obj, string filename)
        {
            string json = UnityEngine.JsonUtility.ToJson(obj);
            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
            {
                sw.Write(json);
            }
            return true;
        }

    }

}