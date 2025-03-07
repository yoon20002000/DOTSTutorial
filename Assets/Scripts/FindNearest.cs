using UnityEngine;

public class FindNearest : MonoBehaviour
{
    void Update()
    {
        foreach (var seekerTransform in Spawner.SeekerTransform)
        {
            Vector3 seekerPos = seekerTransform.localPosition;
            Vector3 nearestTargetPos = default;
            float nearestDistSq = float.MaxValue;
            foreach (var targetTransform in Spawner.TargetTransform)
            {
                Vector3 offset = targetTransform.localPosition - seekerPos;
                float distSq = offset.sqrMagnitude;
                if (distSq < nearestDistSq)
                {
                    nearestDistSq = distSq;
                    nearestTargetPos = targetTransform.localPosition;
                }
            }

            Debug.DrawLine(seekerPos, nearestTargetPos);
        }
    }
}
