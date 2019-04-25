using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using UnityEditorInternal;

namespace EasyBehaviorTree.Creator
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
        private SerializedProperty displayName;
        private SerializedProperty nodeDescription;

        private void Collect()
        {
            nodeTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(NodeBase)) && !t.IsAbstract)).ToArray();
            nodeNames = nodeTypes.Select(t => t.FullName).ToArray();
            assemNames = nodeTypes.Select(t => t.Assembly.GetName().Name).ToArray();
        }

        private void OnEnable()
        {
            Collect();
            nodeDefine = serializedObject.targetObject as NodeDefine;
            nodeFullName = this.serializedObject.FindProperty("nodeFullName");
            assemblyName = this.serializedObject.FindProperty("assemblyName");
            displayName = this.serializedObject.FindProperty("displayName");
            nodeDescription = this.serializedObject.FindProperty("nodeDescription");

            for (int i = 0, len = nodeTypes.Length; i < len; ++i)
            {
                if (nodeNames[i] == nodeFullName.stringValue &&
                    assemNames[i] == assemblyName.stringValue)
                {
                    selected = i;
                }
            }

            OnSelectIndexChanged();
        }

        private void OnDisable()
        {
        }

        private void DrawNodeProperties()
        {
            var properties = NodeDefine.GetNodeParamProperties(nodeTypes[selected]);

            foreach(var property in properties)
            {
                // =============================================================================================================================
                nodeDefine.stringParamSet.TryDrawPropertyForType(property, this.serializedObject.FindProperty("stringParamSet"));
                nodeDefine.floatParamSet.TryDrawPropertyForType(property, this.serializedObject.FindProperty("floatParamSet"));
                nodeDefine.intParamSet.TryDrawPropertyForType(property, this.serializedObject.FindProperty("intParamSet"));
                nodeDefine.boolParamSet.TryDrawPropertyForType(property, this.serializedObject.FindProperty("boolParamSet"));
                nodeDefine.enumParamSet.TryDrawPropertyForType(property, this.serializedObject.FindProperty("enumParamSet"));
                // =============================================================================================================================
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.serializedObject.Update();

            GUILayout.BeginVertical("Box");

            displayName.stringValue = EditorGUILayout.TextField("DisplayName", displayName.stringValue);

            int newSelected = EditorGUILayout.Popup("NodeType", selected, nodeNames);
            if(selected != newSelected)
            {
                selected = newSelected;
                OnSelectIndexChanged();
            }

            DrawNodeProperties();

            GUILayout.EndVertical();

            DrawButtons();

            GUILayout.Label("Description");
            nodeDescription.stringValue = EditorGUILayout.TextArea(nodeDescription.stringValue,GUILayout.MinHeight(50));

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSelectIndexChanged()
        {
            nodeFullName.stringValue = nodeNames[selected];
            assemblyName.stringValue = assemNames[selected];
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();

            if(GUILayout.Button("GoName->NodeName"))
            {
                displayName.stringValue = nodeDefine.gameObject.name;
            }

            if (GUILayout.Button("NodeName->GoName"))
            {
                nodeDefine.gameObject.name = displayName.stringValue;
            }

            /*
            if (GUILayout.Button("Log"))
            {

            }
            */

            GUILayout.EndHorizontal();
        }
    }
}
