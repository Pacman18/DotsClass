using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

namespace DotsStudy
{
    // 월드에 매칭되는 쿼리가 없으면 업데이트 안함 
    [RequireMatchingQueriesForUpdate]
    public partial struct SantaCubeSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SantaCube>(); // 월드에 SantaCube 컴포넌트가 있을 때만 실행
            Debug.Log("SantaCubeSystem created");
        }

        public void OnUpdate(ref SystemState state)
        {

            foreach (var (cube, transform) in SystemAPI.Query<RefRW<SantaCube>, RefRW<LocalTransform>>())
            {
                float dt = SystemAPI.Time.DeltaTime;
                // number를 "초당 도(deg/s)"로 쓰는 예시 — 라디안/초로 바꿔 누적
                ref var c = ref cube.ValueRW;
                c.YawRadians += math.radians((float)c.number) * dt;
                transform.ValueRW.Rotation = quaternion.RotateY(c.YawRadians);
            }

            
        }

        public void OnDestroy(ref SystemState state)
        {
            Debug.Log("SantaCubeSystem destroyed");
        }
    }
}
