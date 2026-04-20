using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

namespace DotsStudy
{
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct SantaCubeMoveSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SantaCube>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                foreach (var (cube, transform) in SystemAPI.Query<RefRW<SantaCube>, RefRW<LocalTransform>>())
                {
                    float dt = SystemAPI.Time.DeltaTime;
                    transform.ValueRW.Position += new float3(0f, 0f, 1f) * dt;
                }
            }

        }
    }


    
    
}


