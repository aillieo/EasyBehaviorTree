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
    [Serializable]
    internal struct NodeParamConfigEntry
    {
        public string typeName;
        public string paramTypeName;
        public bool includeArrayType;
        public bool willGenerate;

        internal NodeParamConfigEntry(string type)
        {
            type = type.Trim(' ', '\n', '\r', '\t');
            typeName = type;
            paramTypeName = GetDefaultParamTypeName(type);
            includeArrayType = true;
            willGenerate = true;
        }

        internal static string GetDefaultParamTypeName(string typeName)
        {
            return typeName.Replace(".","_").Replace("[]","Array");
        }

        internal NodeParamConfigEntry MakeArrayTypeEntry()
        {
            NodeParamConfigEntry ret = new NodeParamConfigEntry(typeName + "[]");
            ret.paramTypeName = paramTypeName + "Array";
            return ret;
        }

    }

    internal class NodeParamConfig : ScriptableSingleton<NodeParamConfig>
    {
        [SerializeField]
        internal DefaultAsset folder;

        [SerializeField]
        [HideInInspector]
        internal NodeParamConfigEntry[] extendedTypes = Array.Empty<NodeParamConfigEntry>();

        [SerializeField]
        [HideInInspector]
        internal NodeParamConfigEntry[] defaultTypes = Array.Empty<NodeParamConfigEntry>();
    }

}
