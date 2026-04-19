using UnityEngine;
using Unity.Collections;
using Unity.Jobs;

public class JobSample : MonoBehaviour
{
    public int count = 1000;   

    private NativeArray<int> _jobnumbers;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _jobnumbers = new NativeArray<int>(count, Allocator.Persistent);
        for (int i = 0; i < _jobnumbers.Length; i++)
        {
            _jobnumbers[i] = i + 1;
        }        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            var job = new MultiplicationJob
            {
                numbers = _jobnumbers,
                results = new NativeArray<int>(count * count, Allocator.TempJob)
            };

            var handle = job.Schedule();
            handle.Complete();

            for (int i = 0; i < job.results.Length; i++)
            {
                Debug.Log(job.results[i]);
            }

            job.results.Dispose();
        }               
    }

    struct MultiplicationJob : IJob
    {
        public NativeArray<int> numbers;
        public NativeArray<int> results;


        public void Execute()
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                for(int j = 0; j < numbers.Length; j++)
                {
                    results[i * numbers.Length + j] = numbers[i] * numbers[j];
                }                
            }
        }

    }
}
