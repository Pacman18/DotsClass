using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

/// <summary>
/// JobSample과 같은 연산(곱셈 표)을 IJobParallelFor로: 바깥 i를 인덱스마다 병렬 실행합니다.
/// </summary>
public class JobSampleParallelFor : MonoBehaviour
{
    public int count = 1000;
    [Tooltip("IJobParallelFor Schedule의 innerloopBatchCount — 한 배치에 묶을 인덱스 개수")]
    public int innerloopBatchCount = 64; // 32~128 권장 프로파일을 보고 설정 

    NativeArray<int> _jobnumbers;

    void Start()
    {
        _jobnumbers = new NativeArray<int>(count, Allocator.Persistent);
        for (int i = 0; i < _jobnumbers.Length; i++)
            _jobnumbers[i] = i + 1;
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space))
            return;

        var results = new NativeArray<int>(count * count, Allocator.TempJob);

        var job = new MultiplicationParallelJob
        {
            numbers = _jobnumbers,
            results = results
        };

        // 첫 번째 인자: 병렬로 나눌 개수(여기서는 행 인덱스 i의 개수) 
        JobHandle handle = job.Schedule(_jobnumbers.Length, innerloopBatchCount);
        handle.Complete();

        for (int i = 0; i < Mathf.Min(8, results.Length); i++)
            Debug.Log($"[ParallelFor] results[{i}]={results[i]}");

        results.Dispose();
    }

    void OnDestroy()
    {
        if (_jobnumbers.IsCreated)
            _jobnumbers.Dispose();
    }

    // 병렬로 도는것은 맞지만 쓰레드는 스케쥴러에 의해 결정된다 
    [BurstCompile]
    struct MultiplicationParallelJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<int> numbers;
        public NativeArray<int> results;

        public void Execute(int i)
        {
            int len = numbers.Length;
            int row = i * len;
            for (int j = 0; j < len; j++)
                results[row + j] = numbers[i] * numbers[j];
        }
    }
}
