using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;


namespace DotsStudy
{
    [RequireMatchingQueriesForUpdate]
    public partial struct BulletMoveSystem : ISystem
    {

        public NativeList<Entity> removeEntities;


        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BulletComponent>();
            removeEntities = new NativeList<Entity>(1000, Allocator.Persistent);
        }        

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (bullet, transform, entity) in SystemAPI.Query<RefRW<BulletComponent>, RefRW<LocalTransform>>().WithEntityAccess())
            {
                transform.ValueRW.Position += new float3(0f, 1f, 0f) * bullet.ValueRO.speed * SystemAPI.Time.DeltaTime;
                bullet.ValueRW.removeTime -= SystemAPI.Time.DeltaTime;
                if(bullet.ValueRO.removeTime <= 0f)
                {
                    removeEntities.Add(entity);
                }
            }

            foreach (var entity in removeEntities)
            {
                state.EntityManager.DestroyEntity(entity);
            }
            
            removeEntities.Clear();
        }

        public void OnDestroy(ref SystemState state)
        {
            if (removeEntities.IsCreated)
                removeEntities.Dispose();
        }
    }



}



