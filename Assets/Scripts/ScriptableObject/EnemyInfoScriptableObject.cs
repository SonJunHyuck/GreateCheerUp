using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitInfoScriptableObject", menuName = "EnemyInfo", order = 1)]
public class EnemyInfoScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class UnitInfo
    {
        public string unitKey;
        public float attack;
        public float speed;
        public float health;
        public float special;
        public int gold;
    }

    public List<UnitInfo> units;

    /// <summary>
    /// 유닛 데이터를 키를 기반으로 검색
    /// </summary>
    public UnitInfo GetUnitInfo(string unitKey)
    {
        return units.Find(unit => unit.unitKey == unitKey);
    }
}