using UnityEngine;
using Unity.Entities;

namespace DotsStudy
{

    public class ECSSingleTone : MonoBehaviour
    {
        public float Difficulty;

        public class Baker : Baker<ECSSingleTone>
        {
            public override void Bake(ECSSingleTone authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new GameConfig { Difficulty = authoring.Difficulty });
            }
        }
    }



    public struct GameConfig : IComponentData
    {
        public float Difficulty;
    }
}

