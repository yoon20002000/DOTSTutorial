using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
[BurstCompile]
public struct FindNearestJob : IJob
{
    [ReadOnly]
    public NativeArray<float3> TargetPosition;
    [ReadOnly]
    public NativeArray<float3> SeekerPosition;

    public NativeArray<float3> NearestTargetPositions;
    public void Execute()
    {
        for(int i = 0; i < SeekerPosition.Length; ++ i)
        {
            float3 seekerPos = SeekerPosition[i];
            float nearestDistSq = float.MaxValue;
            for(int j = 0; j < TargetPosition.Length; ++j)
            {
                float3 targetPos = TargetPosition[j];
                float distSq = math.distancesq(seekerPos, targetPos);
                if(distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    NearestTargetPositions[i] = targetPos;
                }
            }
        }
    }
}