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
    public class NodeParamConfig : ScriptableSingleton<NodeParamConfig>
    {
        public DefaultAsset folder;

        [SerializeField][HideInInspector]
        public string[] typeNames;
    }


#if UNITY_EDITOR
    [CustomEditor(typeof(NodeParamConfig))]
    public class NodeParamConfigEditor : Editor
    {

        ReorderableList reorderableList;

        NodeParamConfig targetNodeParamConfig;

        void OnEnable()
        {
            targetNodeParamConfig = target as NodeParamConfig;

            SerializedProperty serializedProperty = this.serializedObject.FindProperty("typeNames");
            reorderableList = new ReorderableList(this.serializedObject, serializedProperty);
            reorderableList.drawHeaderCallback += rect => GUI.Label(rect, "typeNames");
            reorderableList.elementHeight = EditorGUIUtility.singleLineHeight;
            reorderableList.drawElementCallback += (rect, index, isActive, isFocused) => {
                DrawOneProperty(serializedProperty, rect, index, isActive, isFocused);
            };
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Regenerate!"))
            {
                NodeParamGenerator.RegenerateScriptFiles();
            }
        }

        private void DrawOneProperty(SerializedProperty serializedProperty, Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = serializedProperty.GetArrayElementAtIndex(index);
            Color original = GUI.color;
            if(!NodeParamGenerator.IsValidType(serializedProperty.GetArrayElementAtIndex(index).stringValue))
            {
                GUI.color = Color.red;
            }
            //EditorGUI.PropertyField(rect, element, new GUIContent(index.ToString()));
            EditorGUI.PropertyField(rect, element, GUIContent.none);
            GUI.color = original;
        }


        private void ValidateTypes()
        {

        }

    }
#endif
}
