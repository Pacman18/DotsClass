using UnityEngine;
using Unity.Entities;
using Unity.Transforms; 
using Unity.Mathematics;


namespace DotsStudy
{
    [RequireMatchingQueriesForUpdate]
    public partial struct BulletSpwanerSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<BulletSpawnerComponent>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if(Input.GetKeyDown(KeyCode.F5))
            {
                var bulletSpawner = SystemAPI.GetSingleton<BulletSpawnerComponent>();
                var rng = new Unity.Mathematics.Random(100000);

                for(int i = 0; i < bulletSpawner.spawnCount; i++)
                {
                    var bullet = state.EntityManager.Instantiate(bulletSpawner.bulletPrefab);
                    var transform = state.EntityManager.GetComponentData<LocalTransform>(bullet);
                    // Unity.Mathematics.Random: 시드 기반 struct — Burst 호환. (시드 0은 피함)
                    
                    transform.Position = new float3(
                        rng.NextFloat(-10f, 10f),
                        0,
                        rng.NextFloat(-10f, 10f));

                    state.EntityManager.SetComponentData(bullet, transform);
                    state.EntityManager.SetComponentData(bullet, new BulletComponent { speed = rng.NextFloat(0.5f, 1f), removeTime = rng.NextFloat(0.1f, 1f) });
                }
            }
        }
    }
}
