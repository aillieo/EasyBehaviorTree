using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AillieoUtils.EasyBehaviorTree
{
    public class BytesAssetProcessor : IBTAssetProcessor
    {
        private static BytesAssetProcessor instance;

        public static BehaviorTree LoadBehaviorTree(string filePath)
        {
            if (instance == null)
            {
                instance = new BytesAssetProcessor();
            }
            return instance.Load(filePath);
        }

        public static bool SaveBehaviorTree(BehaviorTree behaviorTree, string filePath)
        {
            if (instance == null)
            {
                instance = new BytesAssetProcessor();
            }
            return instance.Save(behaviorTree,filePath);
        }

        public BehaviorTree Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            BehaviorTree behaviorTree = null;
            using (Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                behaviorTree = formatter.Deserialize(stream) as BehaviorTree;
                stream.Close();
            }
            return behaviorTree;
        }

        public bool Save(BehaviorTree behaviorTree, string filepath)
        {
            using (Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, behaviorTree);
                stream.Close();
                return true;
            }
        }
    }
}
