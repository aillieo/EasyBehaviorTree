using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AillieoUtils.EasyBehaviorTree;

namespace AillieoUtils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<Type> FindImplementations(Type interfaceType, bool interfaceIsGenericType)
        {
            if (interfaceIsGenericType)
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()
                        .Where(t => t.GetInterfaces()
                            .Any(i => i.IsGenericType
                                      && i.GetGenericTypeDefinition() == interfaceType)));
            }
            else
            {
                return AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes()
                        .Where(t => t.GetInterfaces().Contains(interfaceType)));
            }
        }

        public static IEnumerable<FieldInfo> GetNodeParamFields(Type nodeType)
        {
            return nodeType.GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)
                .Where(fi => fi.GetCustomAttribute<NodeParamAttribute>(false) != null);
        }

        public static FieldInfo GetNodeParamField(Type nodeType, string name)
        {
            return GetNodeParamFields(nodeType)
                .Where(fi => fi.Name == name).FirstOrDefault();
        }

        private static readonly Dictionary<string, string> cachedAssemblyQualifiedNameByTypeName = new Dictionary<string, string>();
        public static Type GetType(string typeName)
        {
            Type type = Type.GetType(typeName);
            if (type != null)
            {
                return type;
            }
            string assemblyQualifiedName;
            if (!cachedAssemblyQualifiedNameByTypeName.TryGetValue(typeName, out assemblyQualifiedName))
            {
                foreach (var ass in AppDomain.CurrentDomain.GetAssemblies())
                {
                    type = ass.GetType(typeName);
                    if (type != null)
                    {
                        cachedAssemblyQualifiedNameByTypeName.Add(typeName, type.AssemblyQualifiedName);
                        return type;
                    }
                }
                cachedAssemblyQualifiedNameByTypeName.Add(typeName, null);
            }

            if(!string.IsNullOrEmpty(assemblyQualifiedName))
            {
                return Type.GetType(assemblyQualifiedName);
            }
            UnityEngine.Debug.LogError("Get type failed: " + typeName);
            return null;
        }
    }

}
