using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AillieoUtils.EasyBehaviorTree
{
    public static class ParamInfoProcessor
    {
        public static object Load(Type t, string serializedValue)
        {
            EnsureProcessors();
            return loadFuncs[t].Invoke(
                processorsForType[t],
                new object[] {serializedValue});
        }

        public static string Save(Type t, object value)
        {
            EnsureProcessors();
            return saveFuncs[t].Invoke(
                processorsForType[t],
                new object[] {value}) as string;
        }

        private static Dictionary<Type, object> processorsForType;
        private static Dictionary<Type, MethodInfo> saveFuncs;
        private static Dictionary<Type, MethodInfo> loadFuncs;
        private static void EnsureProcessors()
        {
            if (processorsForType == null)
            {
                processorsForType = ReflectionUtils.FindImplementations(typeof(IParamValueProcessor<>),true)
                    .Where(t=> !t.IsAbstract)
                    .ToDictionary(
                        t => t
                            .GetInterfaces()
                            .First(i => i.IsGenericType
                                        && i.GetGenericTypeDefinition() == typeof(IParamValueProcessor<>))
                            .GetGenericArguments().First(),
                        Activator.CreateInstance);
                saveFuncs = processorsForType.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.GetType().GetMethod("Save"));
                loadFuncs = processorsForType.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.GetType().GetMethod("Load"));
            }
        }
    }
}
