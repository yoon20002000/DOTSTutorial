using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
[BurstCompile]
public struct FindNearestJob : IJobParallelFor
{
    [ReadOnly]
    public NativeArray<float3> TargetPosition;
    [ReadOnly]
    public NativeArray<float3> SeekerPosition;

    public NativeArray<float3> NearestTargetPositions;
    public void Execute(int index)
    {
        float3 seekerPos = SeekerPosition[index];
        float nearestDistSq = float.MaxValue;
        for (int i = 0; i < TargetPosition.Length; ++i)
        {
            float3 targetPos = TargetPosition[i];
            float distSq = math.distancesq(seekerPos, targetPos);
            if (distSq < nearestDistSq)
            {
                nearestDistSq = distSq;
                NearestTargetPositions[index] = targetPos;
            }
        }
    }
}