using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct CannonBallSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecsSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        var cannonBallJob = new CannonBallJob
        {
            ECB = ecsSingleton.CreateCommandBuffer(state.WorldUnmanaged),
            DeltaTime = SystemAPI.Time.DeltaTime,
        };

        cannonBallJob.Schedule();
    }
}

// IJobEntity는 소스 생성을 사용하여 Execute 메서드의 서명에 쿼리를 암시적으로 정의
// 이 경우 암시적 쿼리는 CannonBall 및 LocalTransform 컴포넌트를 가진 모든 엔티티를 찾음.
[BurstCompile]
public partial struct CannonBallJob : IJobEntity
{
    public EntityCommandBuffer ECB;
    public float DeltaTime;

    // Execute는 CannonBall 및 LocalTransform 컴포넌트를 가진 모든 엔티티마다 호출
    void Execute(Entity entity, ref CannonBall cannonBall, ref LocalTransform transform)
    {
        var gravity = new float3(0.0f, -9.82f, 0.0f);

        transform.Position += cannonBall.Velocity * DeltaTime;

        // 땅에 닿은 경우 삭제
        if (transform.Position.y <= 0.0f)
        {
            ECB.DestroyEntity(entity);
        }

        cannonBall.Velocity += gravity * DeltaTime;
    }
}
