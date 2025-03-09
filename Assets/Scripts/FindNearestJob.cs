using System;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEditor;
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

        int startIdx = TargetPosition.BinarySearch(seekerPos, new AxisXComparer { });

        if(startIdx < 0)
        {
            startIdx = ~startIdx;
        }

        if (startIdx >= TargetPosition.Length)
        {
            startIdx = TargetPosition.Length - 1;
        }

        float3 nearestTargetPos = TargetPosition[startIdx];
        float nearestDistSq = math.distancesq(seekerPos, nearestTargetPos);

        // index기준 위쪽으로
        Search(seekerPos, startIdx + 1, TargetPosition.Length, +1,ref nearestTargetPos, ref nearestDistSq);
        // 아래쪽으로 검색
        Search(seekerPos, startIdx - 1, -1 , -1, ref nearestTargetPos, ref nearestDistSq);

        NearestTargetPositions[index] = nearestTargetPos;
    }

    private void Search(float3 seekerPos, int startIdx, int endIdx, int step, ref float3 nearestTargetPos, ref float nearestDistSq)
    {
        for(int i = startIdx; i != endIdx; i += step)
        {
            float3 targetPos = TargetPosition[i];
            float xdiff = seekerPos.x - targetPos.x;
            if(xdiff*xdiff > nearestDistSq)
            {
                break;
            }

            float distSq = math.distancesq(targetPos, seekerPos);

            if(distSq < nearestDistSq)
            {
                nearestDistSq = distSq;
                nearestTargetPos = targetPos;
            }
        }
    }
}
public struct AxisXComparer : IComparer<float3>
{
    public int Compare(float3 x, float3 y)
    {
        return x.x.CompareTo(y.x);
    }
}