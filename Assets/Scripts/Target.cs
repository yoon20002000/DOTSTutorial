using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private Vector3 direction;
    
    void Update()
    {
        transform.localPosition += direction * Time.deltaTime;
    }
}
