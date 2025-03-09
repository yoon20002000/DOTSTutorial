﻿using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Spawner>();
    }
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false;
        var prefab = SystemAPI.GetSingleton<Spawner>().CubePrefab;
        var instances = state.EntityManager.Instantiate(prefab, 10, Allocator.Temp);

        var random = new Random(123);
        foreach(var entity in instances)
        {
            var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
            transform.ValueRW.Position = random.NextFloat3(new float3(10, 10, 10));
            var rotationSpeed = SystemAPI.GetComponentRW<RotationSpeed>(entity);
            rotationSpeed.ValueRW.RadiansPerSecond = math.radians(random.NextFloat(360));
        }
    }
}
