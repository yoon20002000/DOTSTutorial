using Unity.Entities;
using UnityEngine;

public struct Tank : IComponentData
{
    public Entity Turret;
    public Entity Cannon;
}
public class TankAuthoring : MonoBehaviour
{
    [SerializeField]
    private GameObject Turret;
    [SerializeField]
    private GameObject Cannon;
    class Baker : Baker<TankAuthoring>
    {
        public override void Bake(TankAuthoring authoring)
        {
            var entity = GetEntity(authoring, TransformUsageFlags.Dynamic);

            AddComponent(entity, new Tank 
            {
                Turret = GetEntity(authoring.Turret, TransformUsageFlags.Dynamic),
                Cannon = GetEntity(authoring.Cannon, TransformUsageFlags.Dynamic)
            });
        }
    }
}
