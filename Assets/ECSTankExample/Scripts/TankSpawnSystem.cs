using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using Random = Unity.Mathematics.Random;

namespace TankExample
{
    public partial struct TankSpawnSystem : ISystem
    {
        static float4 RandomColor(ref Random random)
        {
            var hue = (random.NextFloat() + 0.618034005f) % 1;
            return (Vector4)Color.HSVToRGB(hue, 1.0f, 1.0f);
        }
        [BurstCompile]
        public void OnCreate(ref SystemState systemState)
        {
            // RequireForUppate는 Config 컴포넌트가 있는 엔티티가
            // 하나 이상 존재하는 경우에만 시스템이 업데이트 됨을 의미
            // 실제로 이 시스템은 Config가 포함된 하위 씬이 로드될 때까지 업데이트 되지 않음
            systemState.RequireForUpdate<Config>();
        }
        [BurstCompile]
        public void OnUpdate(ref SystemState systemState)
        {
            // 시스템을 비활성화하면 추가 업데이트 중지
            // 첫 번째 업데이트에서 이 시스템을 비활성화하면 업데이트가 한 번만 됨.
            systemState.Enabled = false;

            // T Entity가 없거나 2개 이상일 경우 Exception error 발생
            TankExample.Config config = SystemAPI.GetSingleton<TankExample.Config>();

            Random random = new Random(123);

            for (int i = 0; i < config.TankCount; ++i)
            {
                var tankEntity = systemState.EntityManager.Instantiate(config.TankPrefab);

                var color = new URPMaterialPropertyBaseColor { Value =  RandomColor(ref random)};

                // 프리팹에서 인스턴스화된 모든 루트 엔티티는 프리팹 계층 구조를 구성하는
                // 모든 엔티티의 목록인 LinkedEntityGroup 컴포넌가 있음.

                // LinkedEntityGroup은 DynamicBuffer라고 하는 특별한 유형의 컴포넌트
                // 단일 구조체 대신 구조체 값으로 구성된 크기 조절 가능한 배열
                DynamicBuffer<LinkedEntityGroup> linkedEntities = systemState.EntityManager.GetBuffer<LinkedEntityGroup>(tankEntity);
                for (int j = 0; j < linkedEntities.Length; ++j)
                {
                    LinkedEntityGroup targetEntity = linkedEntities[j];
                    // URPMaterialPropertyBaseColor를 보유하고 있는지 확인
                    if (systemState.EntityManager.HasComponent<URPMaterialPropertyBaseColor>(targetEntity.Value))
                    {
                        // 탱크 구성하는 각 엔티티의 생상을 설정
                        systemState.EntityManager.SetComponentData(targetEntity.Value, color);
                        Debug.Log($"X: {color.Value.x} Y: {color.Value.y} Z: {color.Value.z}");
                    }
                }
            }
        }
    }
}