using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySpawnRule", menuName = "ScriptableObjects/Enemy Spawn Rules", order = 2)]
public class SpawnRuleScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class SpawnRule
    {
        public int stageId;            // 스테이지 ID
        public string key;             // 유닛 키
        public int spawnCount;         // 스폰 횟수 (-1이면 무한)
        public int minSpawnCount;      // 최소 스폰 횟수
        public int maxSpawnCount;      // 최대 스폰 횟수
        public float minInterval;      // 최소 스폰 간격
        public float maxInterval;      // 최대 스폰 간격
    }

    public List<SpawnRule> spawnRules; // 해당 스테이지에서의 유닛 스폰 규칙 리스트
}