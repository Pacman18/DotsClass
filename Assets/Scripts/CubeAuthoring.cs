using UnityEngine;
using Unity.Entities;


namespace DotsStudy
{
    public class CubeAuthoring : MonoBehaviour
    {   
        public int number; // 컴포넌트 데이터
        public float size; // 컴포넌트 데이터
        public GameObject cube;


        class Baker : Baker<CubeAuthoring>
        {

            public override void Bake(CubeAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SantaCube
                {
                    number = authoring.number,
                    size = authoring.size,
                    YawRadians = 0f,
                    cube = GetEntity(authoring.cube, TransformUsageFlags.Dynamic),
                });

                Debug.Log("Bake: " + authoring.number);
            }
        }
    }



    public struct SantaCube : IComponentData
    {
        public int number;
        public float size;
        /// <summary>Y축 기준 누적 회전(라디안). 시스템에서 매 프레임 더해 갱신.</summary>
        public float YawRadians;
        public Entity cube;
    }
}

