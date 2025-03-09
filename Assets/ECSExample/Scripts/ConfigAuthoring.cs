using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    public GameObject MonsterPrefab;
    public int NumMonster;

    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new Config()
            {
                MonsterPrefab = GetEntity(authoring.MonsterPrefab, TransformUsageFlags.Dynamic),
                NumMonsters = authoring.NumMonster
            });
        }
    }
}
