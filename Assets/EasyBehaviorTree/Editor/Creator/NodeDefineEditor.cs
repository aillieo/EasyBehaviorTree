using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

namespace AillieoUtils.EasyBehaviorTree.Creator
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
        private SerializedProperty displayName;
        private SerializedProperty nodeDescription;
        private SerializedProperty nodeParams;

        private void Collect()
        {
            nodeTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes().Where(t => t.IsSubclassOf(typeof(NodeBase)) && !t.IsAbstract)).ToArray();
            nodeNames = nodeTypes.Select(t => t.FullName).ToArray();
        }

        private void OnEnable()
        {
            Collect();
            nodeDefine = serializedObject.targetObject as NodeDefine;
            nodeFullName = this.serializedObject.FindProperty("nodeFullName");
            displayName = this.serializedObject.FindProperty("displayName");
            nodeDescription = this.serializedObject.FindProperty("nodeDescription");
            nodeParams = this.serializedObject.FindProperty("nodeParams");

            for (int i = 0, len = nodeTypes.Length; i < len; ++i)
            {
                if (nodeNames[i] == nodeFullName.stringValue)
                {
                    selected = i;
                }
            }

            OnSelectIndexChanged();
        }

        private void OnDisable()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            this.serializedObject.Update();

            displayName.stringValue = EditorGUILayout.TextField("DisplayName", displayName.stringValue);

            int newSelected = EditorGUILayout.Popup("NodeType", selected, nodeNames);
            if(selected != newSelected)
            {
                selected = newSelected;
                OnSelectIndexChanged();
            }

            DrawNodeParams();

            DrawButtons();

            GUILayout.Label("Description");
            nodeDescription.stringValue = EditorGUILayout.TextArea(nodeDescription.stringValue,GUILayout.MinHeight(50));
            GUILayout.Space(5);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSelectIndexChanged()
        {
            nodeFullName.stringValue = nodeNames[selected];

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNodeParams()
        {
            if (selected < 0 || selected >= nodeTypes.Length)
            {
                return;
            }

            string json = nodeParams.stringValue;
            Type t = nodeTypes[selected];
            try
            {
                var obj = JsonUtility.FromJson(json, t);
                if (obj == null)
                {
                    obj = Activator.CreateInstance(t);
                }

                IEnumerable<FieldInfo> fields = t.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.GetCustomAttribute<NodeParamAttribute>() != null)
                    .Where(f => !f.IsInitOnly);

                bool changed = false;

                if (fields.Any())
                {
                    EditorGUILayout.BeginVertical("GroupBox");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Node Params");
                    if (GUILayout.Button("Reset"))
                    {
                        nodeParams.stringValue = string.Empty;
                        return;
                    }

                    EditorGUILayout.EndHorizontal();

                    foreach (var f in fields)
                    {
                        EditorGUILayout.BeginVertical("FrameBox");
                        changed = changed || EditorGUIHelper.DrawField(obj, f);
                        EditorGUILayout.EndVertical();
                    }

                    if (changed)
                    {
                        nodeParams.stringValue = JsonUtility.ToJson(obj);
                    }

                    if (!string.IsNullOrWhiteSpace(nodeParams.stringValue))
                    {
                        EditorGUILayout.TextField(nodeParams.stringValue);
                    }

                    EditorGUILayout.EndVertical();
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }

        private void DrawButtons()
        {
            GUILayout.BeginHorizontal();

            if(nodeDefine.IsRoot())
            {
                if (GUILayout.Button("Tree GoName->NodeName"))
                {
                    TreeGoNameToNodeName();
                }

                if (GUILayout.Button("Tree GoName<-NodeName"))
                {
                    TreeNodeNameToGoName();
                }

            }
            else
            {
                if (GUILayout.Button("GoName->NodeName"))
                {
                    displayName.stringValue = nodeDefine.gameObject.name;
                }

                if (GUILayout.Button("GoName<-NodeName"))
                {
                    nodeDefine.gameObject.name = displayName.stringValue;
                }
            }

            GUILayout.EndHorizontal();
        }

        private void TreeGoNameToNodeName()
        {
            Transform root = nodeDefine.transform;
            var nodeDefines = root.gameObject.GetComponentsInChildren<NodeDefine>();
            foreach (var nd in nodeDefines)
            {
                // exclude self
                if (nd != nodeDefine)
                {
                    var ndSerializedObject = new SerializedObject(nd);
                    var displayNameProperty = ndSerializedObject.FindProperty("displayName");
                    displayNameProperty.stringValue = nd.gameObject.name;
                    ndSerializedObject.ApplyModifiedProperties();
                }
            }
        }

        private void TreeNodeNameToGoName()
        {
            Transform root = nodeDefine.transform;
            var nodeDefines = root.gameObject.GetComponentsInChildren<NodeDefine>();
            foreach (var nd in nodeDefines)
            {
                // exclude self
                if (nd != nodeDefine)
                {
                    var goSerializedObject = new SerializedObject(nd.gameObject);
                    var ndSerializedObject = new SerializedObject(nd);
                    var nameProperty = goSerializedObject.FindProperty("m_Name");
                    var displayNameProperty = ndSerializedObject.FindProperty("displayName");
                    nameProperty.stringValue = displayNameProperty.stringValue;
                    goSerializedObject.ApplyModifiedProperties();
                }
            }
        }

    }
}
