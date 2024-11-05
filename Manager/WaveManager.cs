using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public EnemySpawnRuleScriptableObject stageSpawnRule; // 여러 스테이지의 스폰 규칙을 담을 리스트

    private Dictionary<EnemySpawnRuleScriptableObject.EnemySpawnRule, int> currentCounts;

    [SerializeField] private Transform spawnPosition;

    void Start()
    {
        currentCounts = new Dictionary<EnemySpawnRuleScriptableObject.EnemySpawnRule, int>();

        // 스테이지에 해당하는 스폰 규칙을 찾아서 currentCounts를 초기화
        if (stageSpawnRule != null)
        {
            foreach (var rule in stageSpawnRule.spawnRules)
            {
                // 모든 룰에 대해 현재 카운트를 0으로 초기화
                if (!currentCounts.ContainsKey(rule))
                {
                    currentCounts.Add(rule, 0);
                }
            }

            // 해당 스테이지의 스폰 규칙에 따른 웨이브 스폰 시작
            StartCoroutine(SpawnWavesForStage(stageSpawnRule.stageId));
        }
        else
        {
            DebugWrapper.LogError($"Stage ID {stageSpawnRule.stageId}에 대한 스폰 규칙을 찾을 수 없습니다.");
        }
    }

    IEnumerator SpawnWavesForStage(int stageId)
    {
        if (stageSpawnRule != null)
        {
            // 해당 스테이지의 규칙들을 순차적으로 실행
            foreach (var rule in stageSpawnRule.spawnRules)
            {
                StartCoroutine(SpawnUnitsForRule(rule));
            }
        }
        else
        {
            DebugWrapper.LogError($"Stage ID {stageId}에 대한 스폰 규칙을 찾을 수 없습니다.");
        }

        yield break;
    }

    // 유닛을 스폰하는 코루틴
    IEnumerator SpawnUnitsForRule(EnemySpawnRuleScriptableObject.EnemySpawnRule rule)
    {
        int maxRetryAttempts = 3;  // 최대 재시도 횟수
        WaitForSeconds retryDelay = new (1.0f);  // 재시도 간격 (초)

        while (rule.spawnCount == -1 || currentCounts[rule] < rule.spawnCount)
        {
            // MinInterval과 MaxInterval 사이의 랜덤 값으로 스폰 간격 설정
            float randomInterval = Random.Range(rule.minInterval, rule.maxInterval);
            yield return new WaitForSeconds(randomInterval);

            // MinSpawnCount와 MaxSpawnCount 사이의 랜덤 값으로 스폰 수 결정
            int spawnAmount = Random.Range(rule.minSpawnCount, rule.maxSpawnCount + 1);
            for (int i = 0; i < spawnAmount; i++)
            {
                float spawnPosY = Random.Range(-0.6f, 0.0f);

                // 최대 재시도 횟수 설정
                int retryCount = 0;
                GameObject obj = null;

                // obj를 성공적으로 가져올 때까지 재시도
                while (retryCount < maxRetryAttempts)
                {
                    obj = PoolManager.Instance.EnemyPool.GetFromPool(rule.tag);

                    // obj가 null이 아니면 스폰 성공, 재시도 중단
                    if (obj != null)
                    {
                        break;
                    }

                    // 재시도 실패 시 대기
                    retryCount++;
                    DebugWrapper.LogWarning($"Failed to get object from pool. Retrying {retryCount}/{maxRetryAttempts}...");
                    yield return retryDelay;  // 재시도 간격
                }

                // 재시도 횟수를 초과하면 스폰 중단
                if (obj == null)
                {
                    DebugWrapper.LogError($"Failed to spawn object '{rule.tag}' after {maxRetryAttempts} attempts.");
                    continue;  // 다음 스폰 시도
                }

                // obj가 성공적으로 스폰되었을 때 위치 설정
                obj.GetComponent<Collider2D>().enabled = true;
                obj.transform.position = new Vector3(spawnPosition.position.x, spawnPosY, 0);
            }

            currentCounts[rule] += spawnAmount; // 스폰 횟수 증가

            // 유한 스폰일 경우 다 스폰되면 종료
            if (rule.spawnCount != -1 && currentCounts[rule] >= rule.spawnCount)
            {
                yield break;
            }
        }
    }
}