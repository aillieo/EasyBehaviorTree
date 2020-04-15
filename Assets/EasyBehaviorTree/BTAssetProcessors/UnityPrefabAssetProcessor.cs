using System;
using System.IO;

namespace AillieoUtils.EasyBehaviorTree
{
    public class UnityPrefabAssetProcessor : IBTAssetProcessor<UnityEngine.GameObject>
    {
        private static UnityPrefabAssetProcessor instance;

        public static BehaviorTree LoadBehaviorTree(UnityEngine.GameObject prefab)
        {
            if (instance == null)
            {
                instance = new UnityPrefabAssetProcessor();
            }
            return instance.Load(prefab);
        }

        public static UnityEngine.GameObject SaveBehaviorTree(BehaviorTree behaviorTree)
        {
            if (instance == null)
            {
                instance = new UnityPrefabAssetProcessor();
            }
            return instance.Save(behaviorTree);
        }

        public BehaviorTree Load(UnityEngine.GameObject asset)
        {
            BehaviorTree behaviorTree = CreatorUtils.NewBehaviorTree(null);
            return behaviorTree;
        }

        public UnityEngine.GameObject Save(BehaviorTree behaviorTree)
        {
            throw new NotImplementedException();
        }
    }
}
