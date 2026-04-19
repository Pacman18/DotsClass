using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DotsStudy
{
    /// <summary>
    /// MonoBehaviour에서 가장 기본인 IJob을 쓰는 예제입니다.
    /// (병렬 버전은 IJobParallelFor — 배열을 인덱스별로 나눠 여러 스레드가 돌립니다.)
    /// 빈 오브젝트에 붙이고 플레이하면 콘솔에 샘플 로그가 찍힙니다.
    /// </summary>
    public class SimpleJobMonoExample : MonoBehaviour
    {
        [SerializeField] int count = 4096;

        NativeArray<float> values;

        void Start()
        {
            values = new NativeArray<float>(count, Allocator.Persistent);
            for (int i = 0; i < values.Length; i++)
                values[i] = i + 1f;
        }

        void Update()
        {
            var job = new ScaleJob
            {
                Values = values,
                Factor = Time.deltaTime + 0.5f
            };

            // Schedule()으로 Job을 예약하면, Unity Job System이 워커 스레드에서
            // 아래 struct의 Execute()를 대신 호출합니다. 직접 Execute()를 부르지 않습니다.
            JobHandle handle = job.Schedule();
            handle.Complete();

            // 결과는 Job 끝난 뒤(Complete 이후) 메인 스레드에서 읽어서 로그 — Execute 안에서 Debug.Log 하지 않기
            if (Time.frameCount % 60 == 0 && values.Length > 0)
                Debug.Log($"[결과 샘플] values[0]={values[0]}, values[1]={values[1]}");
        }

        void OnDestroy()
        {
            if (values.IsCreated)
                values.Dispose();
        }

        [BurstCompile]
        struct ScaleJob : IJob
        {
            public NativeArray<float> Values;
            public float Factor;

            public void Execute()
            {
                for (int i = 0; i < Values.Length; i++)
                    Values[i] *= Factor;
            }
        }
    }
}
