
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class FindNearest : MonoBehaviour
{
    NativeArray<float3> TargetPositions;
    NativeArray<float3> SeekerPositions;
    NativeArray<float3> NearestTargetPositions;
    private void Start()
    {
        Spawner spawner = Object.FindFirstObjectByType<Spawner>();
        if(spawner != null)
        {                                                                       // Persistent : 지속되는, 즉 영구 할당자 역할
            TargetPositions = new NativeArray<float3>(spawner.GetNumTargets(), Allocator.Persistent);
            SeekerPositions = new NativeArray<float3>(spawner.GetNumTargets(), Allocator.Persistent);
            NearestTargetPositions = new NativeArray<float3>(spawner.GetNumTargets(), Allocator.Persistent);
        }
    }
    private void OnDestroy()
    {
        TargetPositions.Dispose();
        SeekerPositions.Dispose();
        NearestTargetPositions.Dispose();
    }
    private void Update()
    {
        for(int i = 0; i < TargetPositions.Length; ++i)
        {
            TargetPositions[i] = Spawner.TargetTransform[i].localPosition;
        }
        for (int i = 0; i < TargetPositions.Length; ++i)
        {
            SeekerPositions[i] = Spawner.SeekerTransform[i].localPosition;
        }

        FindNearestJob findJob = new FindNearestJob
        {
            TargetPosition = TargetPositions,
            SeekerPosition = SeekerPositions,
            NearestTargetPositions = NearestTargetPositions
        };

        JobHandle handle = findJob.Schedule(SeekerPositions.Length, 100);
        handle.Complete();

        for(int i = 0; i < NearestTargetPositions.Length; ++i)
        {
            Debug.DrawLine(SeekerPositions[i], NearestTargetPositions[i]);
        }
    }
}
