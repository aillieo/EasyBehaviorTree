using System.Collections.Generic;
using System;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    internal static class NodeParamGenerator
    {

        private static HashSet<string> csTypes = new HashSet<string>()
        {
            "bool",
            "byte",
            "char",
            "decimal",
            "double",
            "float",
            "int",
            "long",
            "object",
            "sbyte",
            "short",
            "string",
            "uint",
            "ulong",
            "ushort",
        };


        internal static void CreateNodeDefine(string folder, IList<NodeParamConfigEntry> entries)
        {

            StringBuilder stringBuilder1 = new StringBuilder();
            StringBuilder stringBuilder2 = new StringBuilder();

            foreach (var entry in entries)
            {
                if (IsTypeNameValid(entry.typeName) && entry.willGenerate)
                {
                    string str0 = entry.safeParamTypeName;
                    string str1 = "m" + entry.safeParamTypeName;
                    stringBuilder1.AppendFormat(CodeTemplate.nodeDefineTemplate1, str0, str1);
                    stringBuilder2.AppendFormat(CodeTemplate.nodeDefineTemplate2, str0, str1, entry.typeName);

                    if (entry.includeArrayType)
                    {
                        NodeParamConfigEntry arrayEntry = entry.MakeArrayTypeEntry();
                        str0 = arrayEntry.safeParamTypeName;
                        str1 = "m" + arrayEntry.safeParamTypeName;
                        stringBuilder1.AppendFormat(CodeTemplate.nodeDefineTemplate1, str0, str1);
                        stringBuilder2.AppendFormat(CodeTemplate.nodeDefineTemplate2, str0, str1, arrayEntry.typeName);
                    }
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

        internal static void CreateOneFile(string folder, NodeParamConfigEntry nodeParamConfigEntry)
        {
            string str0 = nodeParamConfigEntry.typeName;
            string str1 = nodeParamConfigEntry.safeParamTypeName;

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


        internal static bool IsTypeNameValid(string typeName)
        {
            if (string.IsNullOrWhiteSpace(typeName))
            {
                return false;
            }

            if (csTypes.Contains(typeName))
            {
                return true;
            }

            Type t = null;
            var ass = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var a in ass)
            {
                t = a.GetType(typeName);
                if (t != null)
                {
                    break;
                }
            }
            return t != null;
        }


        [MenuItem("EasyBehaviorTree/Regenerate Node Params")]
        internal static void RegenerateScriptFiles()
        {
            NodeParamConfig nodeParamConfig = NodeParamConfig.instance;
            if (nodeParamConfig.folder != null)
            {
                string path = AssetDatabase.GetAssetPath(nodeParamConfig.folder);
                if (AssetDatabase.IsValidFolder(path))
                {
                    FileUtil.DeleteFileOrDirectory(path);
                    Directory.CreateDirectory(path);
                    List<NodeParamConfigEntry> entries = new List<NodeParamConfigEntry>();
                    entries.AddRange(nodeParamConfig.defaultTypes);
                    entries.AddRange(nodeParamConfig.extendedTypes);

                    foreach (var entry in entries)
                    {
                        if (IsTypeNameValid(entry.typeName) && entry.willGenerate)
                        {
                            CreateOneFile(path, entry);

                            if(entry.includeArrayType)
                            {
                                CreateOneFile(path, entry.MakeArrayTypeEntry());
                            }
                        }
                    }
                    
                    CreateNodeDefine(path, entries);
                    AssetDatabase.Refresh();
                }
            }
        }

    }

}
