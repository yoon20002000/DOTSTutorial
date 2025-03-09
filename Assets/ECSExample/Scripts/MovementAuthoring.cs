using Unity.Entities;
using UnityEngine;

public class MovementAuthoring : MonoBehaviour
{
    public Vector3 Direction;

    class Baker : Baker<MovementAuthoring>
    {
        public override void Bake(MovementAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Movement
            {
                Value = authoring.Direction
            });
        }
    }
}
