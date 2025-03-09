using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Transform[] TargetTransform;
    public static Transform[] SeekerTransform;
    [SerializeField]
    private GameObject seekerPrefab;
    [SerializeField]
    private GameObject targetPrefab;
    [SerializeField]
    private int numSeekers;
    public int GetNumSeekers() => numSeekers;
    [SerializeField]
    private int numTargets;
    public int GetNumTargets() => numTargets;
    [SerializeField]
    private Vector2 bounds;
    void Start()
    {
        Random.InitState(123);

        SeekerTransform = new Transform[numSeekers];
        for (int seekerIndex = 0; seekerIndex < SeekerTransform.Length; ++seekerIndex)
        {
            GameObject go = GameObject.Instantiate(seekerPrefab);
            Seeker seeker = go.GetComponent<Seeker>();
            Vector2 dir = Random.insideUnitCircle;
            seeker.SetDirection(dir);
            SeekerTransform[seekerIndex] = go.transform;
            go.transform.localPosition = new Vector3(Random.Range(0,bounds.x),0, Random.Range(0, bounds.y));
        }

        TargetTransform = new Transform[numTargets];
        for(int targetIndex = 0; targetIndex < TargetTransform.Length; ++targetIndex)
        {
            GameObject go = GameObject.Instantiate(targetPrefab);
            Target target = go.GetComponent<Target>();
            Vector2 dir = Random.insideUnitCircle;
            target.SetDirection(dir);
            TargetTransform[targetIndex] = go.transform;
            go.transform.localPosition = new Vector3(Random.Range(0, bounds.x), 0, Random.Range(0, bounds.y));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
