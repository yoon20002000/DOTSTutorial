using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct TankMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState systemState)
    {
        var deltaTime = SystemAPI.Time.DeltaTime;
            
        // LocalTransform 과 Tank 컴포넌트가 있는 각 엔티티에서 LocalTransform 과 Tank에 Access
        foreach(var (transform, entity) in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Tank>().WithNone<Player>().WithEntityAccess())
        {
            float3 pos = transform.ValueRO.Position;
            // 3D 노이즈 함수를 샘플링하는 지점만 수정한다 함. 정확히는 무슨 뜻인지 확인 필요.
            // 이렇게 해야 각 Tank가 다른 슬라이스를 사용하고 각기 다른 무작위 필드를 따라 이동하게 된다 함.
            pos.y = (float)entity.Index;

            var angle = (0.5f + noise.cnoise(pos / 10f)) * 4.0f * math.PI;
            float3 dir = float3.zero;
            math.sincos(angle, out dir.x, out dir.z);

            transform.ValueRW.Position += dir * deltaTime * 5.0f;
            transform.ValueRW.Rotation = quaternion.RotateY(angle);
        }

        var spin = quaternion.RotateY(SystemAPI.Time.DeltaTime * math.PI);
        foreach(var tank in SystemAPI.Query<RefRW<Tank>>())
        {
            var trans = SystemAPI.GetComponentRW<LocalTransform>(tank.ValueRO.Turret);

            trans.ValueRW.Rotation = math.mul(spin, trans.ValueRO.Rotation);
        }
    }
}
