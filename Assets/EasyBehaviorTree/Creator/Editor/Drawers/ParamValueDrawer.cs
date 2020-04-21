using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AillieoUtils.EasyBehaviorTree.Creator
{
    public static class ParamValueDrawer
    {
        public static void Draw(ParamInfo paramInfo)
        {
            EnsureDrawers();
            Type t = paramInfo.type;
            if(drawFuncs.ContainsKey(t) && drawersForType.ContainsKey(t))
            {
                object newValue = drawFuncs[t].Invoke(
                    drawersForType[t],
                    new object[] { paramInfo.value });
                paramInfo.value = newValue;
            }
            else
            {
                UnityEngine.Debug.LogError($"Cant find drawer for type {t.Name},please create a class and implement IParamValueDrawer<{t.Name}>");
            }
        }

        private static Dictionary<Type, object> drawersForType;
        private static Dictionary<Type, MethodInfo> drawFuncs;
        private static void EnsureDrawers()
        {
            if (drawersForType == null)
            {
                drawersForType = ReflectionUtils.FindImplementations(typeof(IParamValueDrawer<>),true)
                    .Where(t=> !t.IsAbstract)
                    .ToDictionary(
                        t => t
                            .GetInterfaces()
                            .First(i => i.IsGenericType
                                        && i.GetGenericTypeDefinition() == typeof(IParamValueDrawer<>))
                            .GetGenericArguments().First(),
                        Activator.CreateInstance);
                drawFuncs = drawersForType.ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value.GetType().GetMethod("Draw"));
            }
        }
    }
}
