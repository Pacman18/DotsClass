using UnityEngine;
using Unity.Entities;

namespace DotsStudy
{
    public class BulletSpawner : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public int spawnCount = 1000;

        public class Baker : Baker<BulletSpawner>
        {
            public override void Bake(BulletSpawner authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new BulletSpawnerComponent { bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic) 
                , spawnCount = authoring.spawnCount });
            }

        }
    }


    public struct BulletSpawnerComponent : IComponentData
    {
        public Entity bulletPrefab;
        public int spawnCount;
    }
}

