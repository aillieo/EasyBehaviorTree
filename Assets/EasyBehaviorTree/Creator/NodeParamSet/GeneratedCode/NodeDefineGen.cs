// This script is generated by *NodeParamGenerator*
// Any modifications in this script will be lost after regeneration.
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public partial class NodeDefine : MonoBehaviour
    {

#if UNITY_EDITOR

        // =============================================================================================================================

        [HideInInspector][SerializeField]
        public EnumParamSet mEnumParamSet = new EnumParamSet();
        [HideInInspector][SerializeField]
        public StringParamSet mStringParamSet = new StringParamSet();
        [HideInInspector][SerializeField]
        public StringArrayParamSet mStringArrayParamSet = new StringArrayParamSet();
        [HideInInspector][SerializeField]
        public IntParamSet mIntParamSet = new IntParamSet();
        [HideInInspector][SerializeField]
        public IntArrayParamSet mIntArrayParamSet = new IntArrayParamSet();
        [HideInInspector][SerializeField]
        public FloatParamSet mFloatParamSet = new FloatParamSet();
        [HideInInspector][SerializeField]
        public FloatArrayParamSet mFloatArrayParamSet = new FloatArrayParamSet();
        [HideInInspector][SerializeField]
        public BoolParamSet mBoolParamSet = new BoolParamSet();
        [HideInInspector][SerializeField]
        public BoolArrayParamSet mBoolArrayParamSet = new BoolArrayParamSet();
        [HideInInspector][SerializeField]
        public LongParamSet mLongParamSet = new LongParamSet();
        [HideInInspector][SerializeField]
        public LongArrayParamSet mLongArrayParamSet = new LongArrayParamSet();
        [HideInInspector][SerializeField]
        public UnityEngine_Vector3ParamSet mUnityEngine_Vector3ParamSet = new UnityEngine_Vector3ParamSet();
        [HideInInspector][SerializeField]
        public UnityEngine_Vector3ArrayParamSet mUnityEngine_Vector3ArrayParamSet = new UnityEngine_Vector3ArrayParamSet();
        [HideInInspector][SerializeField]
        public UnityEngine_Vector2ParamSet mUnityEngine_Vector2ParamSet = new UnityEngine_Vector2ParamSet();
        [HideInInspector][SerializeField]
        public UnityEngine_Vector2ArrayParamSet mUnityEngine_Vector2ArrayParamSet = new UnityEngine_Vector2ArrayParamSet();

        // =============================================================================================================================

        private NodeDefine()
        {
            cachedMappings = new Dictionary<Type, NodeParamSetAndName>()
            {
                // =============================================================================================================================

                {typeof(Enum),new NodeParamSetAndName() { set = mEnumParamSet,name = "mEnumParamSet" } },
                {typeof(string),new NodeParamSetAndName(){set=mStringParamSet,name = "mStringParamSet" } },
                {typeof(string[]),new NodeParamSetAndName(){set=mStringArrayParamSet,name = "mStringArrayParamSet" } },
                {typeof(int),new NodeParamSetAndName(){set=mIntParamSet,name = "mIntParamSet" } },
                {typeof(int[]),new NodeParamSetAndName(){set=mIntArrayParamSet,name = "mIntArrayParamSet" } },
                {typeof(float),new NodeParamSetAndName(){set=mFloatParamSet,name = "mFloatParamSet" } },
                {typeof(float[]),new NodeParamSetAndName(){set=mFloatArrayParamSet,name = "mFloatArrayParamSet" } },
                {typeof(bool),new NodeParamSetAndName(){set=mBoolParamSet,name = "mBoolParamSet" } },
                {typeof(bool[]),new NodeParamSetAndName(){set=mBoolArrayParamSet,name = "mBoolArrayParamSet" } },
                {typeof(long),new NodeParamSetAndName(){set=mLongParamSet,name = "mLongParamSet" } },
                {typeof(long[]),new NodeParamSetAndName(){set=mLongArrayParamSet,name = "mLongArrayParamSet" } },
                {typeof(UnityEngine.Vector3),new NodeParamSetAndName(){set=mUnityEngine_Vector3ParamSet,name = "mUnityEngine_Vector3ParamSet" } },
                {typeof(UnityEngine.Vector3[]),new NodeParamSetAndName(){set=mUnityEngine_Vector3ArrayParamSet,name = "mUnityEngine_Vector3ArrayParamSet" } },
                {typeof(UnityEngine.Vector2),new NodeParamSetAndName(){set=mUnityEngine_Vector2ParamSet,name = "mUnityEngine_Vector2ParamSet" } },
                {typeof(UnityEngine.Vector2[]),new NodeParamSetAndName(){set=mUnityEngine_Vector2ArrayParamSet,name = "mUnityEngine_Vector2ArrayParamSet" } },

                // =============================================================================================================================
            };
        }
#endif

    }
}