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

namespace AillieoUtils
{

    public static class FileUtils
    {
        public static string Md5(string source)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(source);
            byte[] md5Data = md5.ComputeHash(data, 0, data.Length);
            md5.Clear();

            string destString = "";
            for (int i = 0; i < md5Data.Length; i++)
            {
                destString += System.Convert.ToString(md5Data[i], 16).PadLeft(2, '0');
            }
            destString = destString.PadLeft(32, '0');
            return destString;
        }


        public static string Md5file(string file)
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                fs.Close();
                fs.Dispose();
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Md5file() fail, error:" + ex.Message);
            }
        }

        public static void WriteBytesToDisk(string filename, byte[] bytes)
        {
            if (filename.Contains("/"))
            {
                string[] stringArr = filename.Split('/');
                string curFolder = stringArr[0];
                for (int i = 0; i < stringArr.Length - 1; i++)
                {
                    curFolder += "/";
                    curFolder += stringArr[i];
                    if (!Directory.Exists(curFolder))
                    {
                        Directory.CreateDirectory(curFolder);
                    }
                }
            }

            try
            {
                File.WriteAllBytes(filename, bytes);
            }
            catch (Exception ex)
            {
                Debug.LogError("WriteBytesToDisk Fail: " + ex);
            }
        }

        public static void RemoveFileFromDisk(string filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
        }

        public static void RemoveDirectoryFromDisk(string pathName)
        {
            if (Directory.Exists(pathName))
            {
                Directory.Delete(pathName, true);
            }
        }

        public static string[] GetAllFilenamesInDirectory(string pathName)
        {
            if (Directory.Exists(pathName))
            {
                DirectoryInfo d = new DirectoryInfo(pathName);
                var fileInfos = d.GetFiles();
                int len = fileInfos.Length;
                string[] ret = new string[len];
                for (int i = 0; i < len; ++i)
                {
                    ret[i] = fileInfos[i].Name;
                }
                return ret;
            }
            return null;
        }

        public static byte[] LoadBytesFromDisk(string filename)
        {
            if (System.IO.File.Exists(filename))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filename);
                return bytes;
            }
            else
            {
                return null;
            }
        }

        public static bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public static bool DirectoryExists(string pathName)
        {
            return Directory.Exists(pathName);
        }

    }

}
