using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
using System.IO;

namespace EasyBehaviorTree.Creator
{
    public static class NodeParamGenerator
    {

        public static void CreateNodeDefine(string folder, string[] typeNames)
        {

            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();

            foreach (var typeName in typeNames)
            {
                if (IsValidType(typeName))
                {
                    string str0 = GetTypeName(typeName, true);
                    string str1 = GetTypeName(typeName, false);

                    stringBuilder1.AppendFormat(CodeTemplate.nodeDefineTemplate1, str0, str1);
                    stringBuilder2.AppendFormat(CodeTemplate.nodeDefineTemplate2, str0, str1, typeName);
                }
            }

            string filePath = Path.Combine(folder, "NodeDefineGen.cs");

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(CodeTemplate.head);
                    sw.Write(CodeTemplate.nodeDefineTemplate0, stringBuilder1.ToString(), stringBuilder2.ToString());
                    sw.Flush();
                }
            }

        }

        public static void CreateOneFile(string folder, string typeName)
        {
            string str0 = typeName;
            string str1 = GetTypeName(typeName, true);

            string filePath = Path.Combine(folder, string.Format(CodeTemplate.filename, str1));

            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(CodeTemplate.head);
                    sw.Write(CodeTemplate.nodeParamTempate, str0, str1);
                    sw.Flush();
                }
            }
        }

        private static string GetTypeName(string input, bool upper)
        {
            string ret = input.Trim(' ', '\n', '\r', '\t');
            ret = ret.Replace("[]", "Array");
            int lastDot = ret.LastIndexOf('.');
            if (lastDot != -1)
            {
                ret = ret.Substring(lastDot + 1);
            }
            if (upper)
            {
                ret = ret.Substring(0, 1).ToUpper() + ret.Substring(1);
            }
            else
            {
                ret = ret.Substring(0, 1).ToLower() + ret.Substring(1);
            }
            return ret;
        }

        public static bool IsValidType(string typeName)
        {
            return true;
        }

        [MenuItem("EasyBehaviorTree/Regenerate Node Params")]
        public static void RegenerateScriptFiles()
        {
            NodeParamConfig nodeParamConfig = NodeParamConfig.instance;
            if (nodeParamConfig.folder != null)
            {
                string path = AssetDatabase.GetAssetPath(nodeParamConfig.folder);
                if (AssetDatabase.IsValidFolder(path))
                {
                    FileUtil.DeleteFileOrDirectory(path);
                    Directory.CreateDirectory(path);
                    foreach (var typeName in nodeParamConfig.typeNames)
                    {
                        if (IsValidType(typeName))
                        {
                            CreateOneFile(path, typeName);
                        }
                    }
                    CreateNodeDefine(path, nodeParamConfig.typeNames);
                    AssetDatabase.Refresh();
                }
            }
        }

    }

}
