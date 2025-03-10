using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct ShootingSystem : ISystem
{
    private float timer;
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        timer -= SystemAPI.Time.DeltaTime;
        if(timer > 0)
        {
            return;
        }
        timer = 0.3f;

        var config = SystemAPI.GetSingleton<TankExample.Config>();

        var ballTransform = state.EntityManager.GetComponentData<LocalTransform>(config.CannonBallPrefab);

        foreach(var (tank, transform, color) in SystemAPI.Query<RefRO<Tank>, RefRO<LocalToWorld>, RefRO<URPMaterialPropertyBaseColor>>())
        {
            Entity cannonBallEntity = state.EntityManager.Instantiate(config.CannonBallPrefab);

            // 포탄 색 변경
            state.EntityManager.SetComponentData(cannonBallEntity, color.ValueRO);

            var cannonTransform = state.EntityManager.GetComponentData<LocalToWorld>(tank.ValueRO.Cannon);
            ballTransform.Position = cannonTransform.Position;

            // 새 포탄 생성 위치를 생성 지점과 일치하도록 설정
            state.EntityManager.SetComponentData(cannonBallEntity, ballTransform);

            // 대포 포탄 속도 설정
            state.EntityManager.SetComponentData(cannonBallEntity, new CannonBall { Velocity = math.normalize(cannonTransform.Up) * 12.0f });
        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
