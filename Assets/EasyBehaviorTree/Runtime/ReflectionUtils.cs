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
    }

}
