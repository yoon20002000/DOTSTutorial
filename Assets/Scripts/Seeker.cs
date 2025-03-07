using System;
using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir;
    }

    void Update()
    {
        transform.localPosition += direction * Time.deltaTime;
    }
}
