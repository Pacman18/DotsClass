using UnityEngine;
using Unity.Entities;
using DotsStudy;

namespace DotsStudy
{   
    [RequireMatchingQueriesForUpdate]
    public partial struct SingletoneSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GameConfig>();
            Debug.Log("SingletoneSystem created");
        }

        public void OnUpdate(ref SystemState state)
        {
            var gameConfig = SystemAPI.GetSingleton<GameConfig>();
            Debug.Log("GameConfig: " + gameConfig.Difficulty);            
        }
    }
}
