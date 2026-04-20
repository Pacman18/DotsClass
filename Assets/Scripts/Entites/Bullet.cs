using UnityEngine;
using Unity.Entities;


namespace DotsStudy
{
    public class Bullet : MonoBehaviour
    {

        public class Baker : Baker<Bullet>
        {            
            public override void Bake(Bullet authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new BulletComponent { speed = Random.Range(0.5f, 1f), removeTime = Random.Range(0.5f, 1f)});
            }
        }
    }


    public struct BulletComponent : IComponentData
    {
        public float speed;
        public float removeTime;
    }
}


