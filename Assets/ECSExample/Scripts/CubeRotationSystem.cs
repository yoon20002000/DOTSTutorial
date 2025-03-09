using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct CubeRotationSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
        // RefRW : Read/ Write 참조, RefRO : Read 참조
        foreach(var(transform, rotationspeed) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotationSpeed>>())
        {
            var radian = rotationspeed.ValueRO.RadiansPerSecond * deltaTime;
            transform.ValueRW = transform.ValueRW.RotateY(radian);
        }
    }   
}
