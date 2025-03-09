using Unity.Entities;
using UnityEngine;

public struct Config : IComponentData
{
    public Entity MonsterPrefab;
    public int NumMonsters;
}
