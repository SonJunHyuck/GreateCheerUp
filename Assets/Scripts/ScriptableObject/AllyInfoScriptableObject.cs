using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitInfoScriptableObject", menuName = "Ready/UnitInfo", order = 1)]
public class AllyInfoScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class UnitInfo
    {
        public string unitKey;
        public string unitName;
        public GameObject unitUIPrefab;
        public Sprite unitIcon;
        public string description;
        public int cost;
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