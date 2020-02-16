using UnityEngine;
using System;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

#if UNITY_EDITOR

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

            GUILayout.BeginVertical("Box");

            displayName.stringValue = EditorGUILayout.TextField("DisplayName", displayName.stringValue);

            int newSelected = EditorGUILayout.Popup("NodeType", selected, nodeNames);
            if(selected != newSelected)
            {
                selected = newSelected;
                OnSelectIndexChanged();
            }

            DrawNodeFields();

            DrawButtons();

            GUILayout.Label("Description");
            nodeDescription.stringValue = EditorGUILayout.TextArea(nodeDescription.stringValue,GUILayout.MinHeight(50));
            GUILayout.Space(5);
            GUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnSelectIndexChanged()
        {
            nodeFullName.stringValue = nodeNames[selected];

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawNodeFields()
        {
            nodeDefine.TryDrawFields(ReflectionUtils.GetNodeParamFields(nodeTypes[selected]).ToArray(),serializedObject);
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
#endif
