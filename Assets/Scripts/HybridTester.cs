using UnityEngine;
using Unity.Entities;
using Unity.Collections;
using DotsStudy;

public class HybridTester : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.F1))
        {
            var world = World.DefaultGameObjectInjectionWorld;

            if(world == null) 
                return;

            var entityManager = world.EntityManager;

            var entityQuery = entityManager.CreateEntityQuery(typeof(SantaCube));

            if(entityQuery.IsEmpty)
                return;

            var entities = entityQuery.ToEntityArray(Allocator.Temp);

            foreach(var entity in entities)
            {
                var santaCube = entityManager.GetComponentData<SantaCube>(entity);
                Debug.Log("SantaCube: " + santaCube.number);
            }           

            entities.Dispose(); // 반드시 해제해줘야함.
        }
        
    }
}
