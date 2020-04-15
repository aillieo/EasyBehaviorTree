using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace AillieoUtils.EasyBehaviorTree
{
    public class XmlAssetProcessor : IBTAssetProcessor<string>
    {
        private static XmlAssetProcessor instance;

        public static BehaviorTree LoadBehaviorTree(string xml)
        {
            if (instance == null)
            {
                instance = new XmlAssetProcessor();
            }
            return instance.Load(xml);
        }

        public static string SaveBehaviorTree(BehaviorTree behaviorTree)
        {
            if (instance == null)
            {
                instance = new XmlAssetProcessor();
            }
            return instance.Save(behaviorTree);
        }

        public BehaviorTree Load(string asset)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(asset);
            XmlElement root = xmlDoc.SelectSingleNode("root") as XmlElement;
            BehaviorTree behaviorTree = CreatorUtils.NewBehaviorTree(XmlElementToBTNode(root));
            return behaviorTree;
        }

        public string Save(BehaviorTree behaviorTree)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.AppendChild(BTNodeToXmlElement(xmlDoc, behaviorTree.root));
            return xmlDoc.ToString();
        }

        private static NodeBase XmlElementToBTNode(XmlElement xmlEle, NodeParent nodeParent = null)
        {
            string type = xmlEle.GetAttribute("type");
            Type t = ReflectionUtils.GetType(type);
            if (t == null)
            {
                return null;
            }
            NodeBase node = Activator.CreateInstance(t) as NodeBase;
            if (node == null)
            {
                return null;
            }

            node.name = xmlEle.Name;

            if (nodeParent != null)
            {
                nodeParent.AddChild(node);
            }

            XmlNodeList childNodes = xmlEle.ChildNodes;
            if (childNodes.Count > 0)
            {
                foreach (var xmlNode in childNodes)
                {
                    XmlElement childEle = xmlNode as XmlElement;
                    switch (childEle.Name)
                    {
                        case "param":
                            foreach (var xmlParam in childEle.ChildNodes)
                            {
                                XmlElement xe = xmlParam as XmlElement;
                                if (xe != null)
                                {
                                    string name = xe.Name;
                                    string typeInfo = xe.GetAttribute("type");
                                    string serializedValue = xe.GetAttribute("value");
                                    object value = ParamInfoProcessor.Load(ReflectionUtils.GetType(typeInfo), serializedValue);
                                    node.GetType().GetField(name,BindingFlags.Instance|BindingFlags.NonPublic|BindingFlags.Public).SetValue(node,value);
                                }
                            }
                            break;
                        case "children":
                            foreach (var xmlChild in childEle.ChildNodes)
                            {
                                XmlElementToBTNode(xmlChild as XmlElement, node as NodeParent);
                            }
                            break;
                    }
                }
            }
            return node;
        }

        private static XmlElement BTNodeToXmlElement(XmlDocument xmlDoc, NodeBase node)
        {
            string nodeName = node.name;
            if (string.IsNullOrWhiteSpace(nodeName))
            {
                nodeName = node.GetType().Name;
            }
            XmlElement xmlEle = xmlDoc.CreateElement(nodeName);
            xmlEle.SetAttribute("type", node.GetType().FullName);

            ParamInfo[] paramInfo = CreatorUtils.ExtractParamInfo(node);
            if (paramInfo.Length > 0)
            {
                XmlElement xmlEleParam = xmlDoc.CreateElement("param");
                xmlEle.AppendChild(xmlEleParam);
                foreach (var param in paramInfo)
                {
                    XmlElement xmlEleOneParam = xmlDoc.CreateElement(param.name);
                    xmlEleOneParam.SetAttribute("type", param.type.FullName);
                    xmlEleOneParam.SetAttribute("value", ParamInfoProcessor.Save(param.type,param.value));
                    xmlEleParam.AppendChild(xmlEleOneParam);
                }
            }

            NodeParent nodeParent = node as NodeParent;
            if (nodeParent != null)
            {
                XmlElement xmlEleChildren = xmlDoc.CreateElement("children");
                xmlEle.AppendChild(xmlEleChildren);
                foreach (var child in nodeParent.Children)
                {
                    xmlEleChildren.AppendChild(BTNodeToXmlElement(xmlDoc, child));
                }
            }
            return xmlEle;
        }
    }
}
