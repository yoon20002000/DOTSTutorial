using Unity.Entities;
using UnityEngine;

namespace TankExample
{
    public struct Config : IComponentData
    {
        public Entity TankPrefab;
        public Entity CannonBallPrefab;
        public int TankCount;
    }
    public class ConfigAuthoring : MonoBehaviour
    {
        [SerializeField]
        private GameObject tankPrefab;
        [SerializeField]
        private GameObject cannonBallPrefab;
        [SerializeField]
        private int tankCount;

        class Baker : Baker<ConfigAuthoring>
        {
            public override void Bake(ConfigAuthoring authoring)
            {
                Entity entity = GetEntity(authoring, TransformUsageFlags.None);
                AddComponent(entity, new Config
                {
                    TankPrefab = GetEntity(authoring.tankPrefab,TransformUsageFlags.Dynamic),
                    CannonBallPrefab = GetEntity(authoring.cannonBallPrefab, TransformUsageFlags.Dynamic),
                    TankCount = authoring.tankCount
                });
            }
        }
    }
}
