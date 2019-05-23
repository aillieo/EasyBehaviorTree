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

    [CustomPropertyDrawer(typeof(NodeParamConfigEntry))]
    internal class NodeParamConfigEntryDrawer: PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float x = position.x;
            float y = position.y;
            float width = position.width;
            float height = position.height;
            float widthUnit = width / 4;

            EditorGUI.PropertyField(new Rect(x, y, widthUnit, height), property.FindPropertyRelative("typeName"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(x + widthUnit, y, widthUnit, height), property.FindPropertyRelative("paramTypeName"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(x + widthUnit * 2, y, widthUnit, height), property.FindPropertyRelative("includeArrayType"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(x + widthUnit * 3, y, widthUnit, height), property.FindPropertyRelative("willGenerate"), GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(NodeParamConfig))]
    public class NodeParamConfigEditor : Editor
    {

        ReorderableList reorderableListDefault;
        ReorderableList reorderableListExtended;

        NodeParamConfig targetNodeParamConfig;

        void OnEnable()
        {
            targetNodeParamConfig = target as NodeParamConfig;

            reorderableListDefault = InitiReorderableList(serializedObject.FindProperty("defaultTypes"));
            reorderableListExtended = InitiReorderableList(serializedObject.FindProperty("extendedTypes"));

        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUI.BeginDisabledGroup(false);
            reorderableListDefault.DoLayoutList();
            EditorGUI.EndDisabledGroup();

            reorderableListExtended.DoLayoutList();


            serializedObject.ApplyModifiedProperties();

            if(GUILayout.Button("Regenerate!"))
            {
                NodeParamGenerator.RegenerateScriptFiles();
            }
        }

        private ReorderableList InitiReorderableList(SerializedProperty serializedProperty)
        {
            ReorderableList reorderableList = new ReorderableList(this.serializedObject, serializedProperty);
            reorderableList.drawHeaderCallback += DrawTypeHeader;
            reorderableList.elementHeightCallback += (index) => EditorGUIUtility.singleLineHeight;
            reorderableList.drawElementCallback += (rect, index, isActive, isFocused) => {
                DrawOneProperty(serializedProperty, rect, index, isActive, isFocused);
            };
            return reorderableList;
        }

        private void DrawTypeHeader(Rect rect)
        {
            float x = rect.x;
            float y = rect.y;
            float width = rect.width;
            float height = rect.height;
            float widthUnit = width / 4;
            GUI.Label(new Rect(x, y, widthUnit, height), "typeName");
            GUI.Label(new Rect(x + widthUnit, y, widthUnit, height), "paramTypeName");
            GUI.Label(new Rect(x + widthUnit * 2, y, widthUnit, height), "includeArrayType");
            GUI.Label(new Rect(x + widthUnit * 3, y, widthUnit, height), "willGenerate");
        }

        private void DrawOneProperty(SerializedProperty serializedProperty, Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = serializedProperty.GetArrayElementAtIndex(index);
            Color original = GUI.color;
            //if(!NodeParamGenerator.IsValidType(serializedProperty.GetArrayElementAtIndex(index).stringValue))
            //{
            //    GUI.color = Color.red;
            //}
            EditorGUI.PropertyField(rect, element, GUIContent.none);
            GUI.color = original;
        }


        private void ValidateTypes()
        {

        }

    }
#endif
}
