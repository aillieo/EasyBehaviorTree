using System;

namespace AillieoUtils.EasyBehaviorTree
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class NodeIconAttribute : Attribute
    {
        public NodeIconAttribute(string iconPath)
        {
            this.iconPath = iconPath;
        }
        public string iconPath;
    }
}
