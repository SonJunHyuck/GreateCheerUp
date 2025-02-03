using UnityEngine;

public class AllySpawnController : MonoBehaviour
{
    private void OnEnable()
    {
        Observer.Instance.onRequestSpawnAlly += SpawnUnit;
    }

    private void OnDisable()
    {
        Observer.Instance.onRequestSpawnAlly -= SpawnUnit;
    }

    public void InitUnit(string unitKey, GameObject unit)
    {        
        int grade = PlayerDataManager.GetUnitGrade(unitKey);
        UnitGradeScriptableObject.GradeInfo gradeInfo = DataManager.Instance.GetGradeInfo(unitKey, grade);
        unit.GetComponent<UnitController>().Init(unitKey, gradeInfo.attack, gradeInfo.speed, gradeInfo.health);
    }

    // Key를 이용해 소환, Button에 이벤트 호출
    public bool SpawnUnit(string key)
    {
        // Call the GetFromPool function to get the unit from the object pool
        GameObject unit = PoolManager.Instance.UnitPool.GetGameObject(key);

        if (unit != null)
        {
            InitUnit(key, unit);

            // Set the unit's position to the spawn location and activate it
            unit.transform.position = transform.position;
            unit.GetComponent<Collider2D>().enabled = true;
            unit.SetActive(true);

            Debug.Log($"{key} has been spawned at {transform.position}.");

            return true;
        }
        else
        {
            Debug.LogWarning($"No unit found in the pool with key: {key}");

            return false;
        }
    }
}