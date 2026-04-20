using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace DotsStudy
{
    /// <summary>
    /// 키 입력으로 SantaCube 엔티티 하나를 복제합니다. (Burst 불가 — UnityEngine.Input)
    /// </summary>
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct SantaCubeDuplicateSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SantaCube>();
        }

        public void OnUpdate(ref SystemState state)
        {
            // input에는 burst 불가 
            if (!Input.GetKeyDown(KeyCode.Space))
                return;

            var query = state.GetEntityQuery(
                ComponentType.ReadOnly<SantaCube>(),
                ComponentType.ReadOnly<LocalTransform>());

            if (query.IsEmpty)
                return;

            using (var entities = query.ToEntityArray(Allocator.Temp))
            {
                if (entities.Length == 0)
                    return;

                var src = SystemAPI.GetComponent<SantaCube>(entities[0]).cube;
                var em = state.EntityManager;
                var clone = em.Instantiate(src);

                if (!em.HasComponent<LocalTransform>(clone))
                    return;

                var lt = em.GetComponentData<LocalTransform>(clone);
                lt.Position += new float3(2f, 0f, 0f);
                em.SetComponentData(clone, lt);

                if (em.HasComponent<SantaCube>(clone))
                {
                    var cube = em.GetComponentData<SantaCube>(clone);
                    cube.YawRadians = 0f;
                    em.SetComponentData(clone, cube);
                }
            }
        }
    }
}
