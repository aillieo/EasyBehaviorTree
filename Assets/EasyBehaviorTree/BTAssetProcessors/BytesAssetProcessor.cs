using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AillieoUtils.EasyBehaviorTree
{
    public class BytesAssetProcessor : IBTAssetProcessor<byte[]>
    {
        private static BytesAssetProcessor instance;

        public static BehaviorTree LoadBehaviorTree(byte[] bytes)
        {
            if (instance == null)
            {
                instance = new BytesAssetProcessor();
            }
            return instance.Load(bytes);
        }

        public static byte[] SaveBehaviorTree(BehaviorTree behaviorTree)
        {
            if (instance == null)
            {
                instance = new BytesAssetProcessor();
            }
            return instance.Save(behaviorTree);
        }

        public BehaviorTree Load(byte[] asset)
        {
            BehaviorTree behaviorTree = null;
            using (Stream stream = new MemoryStream(asset))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                behaviorTree = formatter.Deserialize(stream) as BehaviorTree;
                stream.Close();
            }
            return behaviorTree;
        }

        public byte[] Save(BehaviorTree behaviorTree)
        {
            byte[] bytes = null;
            using (Stream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, behaviorTree);
                BinaryReader binaryReader = new BinaryReader(stream);
                binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);
                bytes = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                stream.Close();
            }
            return bytes;
        }
    }
}
