using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;

namespace EasyBehaviorTree
{

    [CustomEditor(typeof(NodeDefine))]
    public class NodeDefineEditor : Editor
    {
        Type[] nodeTypes;
        string[] nodeNames;
        string[] assemNames;
        int selected;

        NodeDefine nodeDefine;

        private SerializedProperty nodeFullName;
        private SerializedProperty assemblyName;
        private SerializedProperty stringParamSet;

        private void Collect()
        {
            nodeTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(NodeBase)) && !t.IsAbstract)).ToArray();
            nodeNames = nodeTypes.Select(t => t.FullName).ToArray();
            assemNames = nodeTypes.Select(t => t.Assembly.GetName().Name).ToArray();
        }

        private void OnEnable()
        {
            nodeDefine = serializedObject.targetObject as NodeDefine;
            Collect();
            nodeFullName = this.serializedObject.FindProperty("nodeFullName");
            assemblyName = this.serializedObject.FindProperty("assemblyName");
            stringParamSet = this.serializedObject.FindProperty("stringParamSet");

            for (int i = 0, len = nodeTypes.Length; i < len; ++i)
            {
                if (nodeNames[i] == nodeFullName.stringValue &&
                    assemNames[i] == assemblyName.stringValue)
                {
                    selected = i;
                }
            }
        }

        private void OnDisable()
        {
        }

        private void DrawNodeProperties()
        {
            var properties = nodeTypes[selected].GetProperties();
            foreach(var p in properties)
            {
                if(p.PropertyType == typeof(string))
                {
                    DrawStringProperty(p);
                }
            }
        }

        private void DrawStringProperty(PropertyInfo propertyInfo)
        {
            string propertyName = propertyInfo.Name;
            GUILayout.Label(propertyName);

            string value = nodeDefine.stringParamSet[propertyName];
            string newValue = EditorGUILayout.TextField(value);
            if(newValue != value)
            {
                var array = stringParamSet.FindPropertyRelative("stringParams");
                var index = nodeDefine.stringParamSet.GetIndexOfKey(propertyName);
                var param = array.GetArrayElementAtIndex(index);
                var paramValue = param.FindPropertyRelative("value");
                paramValue.stringValue = newValue;
            }
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.serializedObject.Update();


            GUILayout.BeginVertical("Box");

            int newSelected = EditorGUILayout.Popup("NodeType", selected, nodeNames);
            if(selected != newSelected)
            {
                selected = newSelected;
                nodeFullName.stringValue = nodeNames[selected];
                assemblyName.stringValue = assemNames[selected];
            }

            DrawNodeProperties();

            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
