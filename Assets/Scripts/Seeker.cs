using UnityEngine;

public class Seeker : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;

    void Update()
    {
        transform.localPosition += direction * Time.deltaTime;
    }
}
