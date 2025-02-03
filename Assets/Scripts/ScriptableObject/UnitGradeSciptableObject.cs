using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitGradeScriptableObject", menuName = "Ready/UnitGrade")]
public class UnitGradeScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class GradeInfo
    {
        public string unitKey;       // 유닛 키
        public int grade;            // 유닛 등급
        public float attack;         // 공격력
        public float speed;          // 속도
        public float health;         // 체력
        public float special;       // 특수 능력 설명
        public int upgradeGold;
    }

    [SerializeField]
    public List<GradeInfo> gradeInfoList = new List<GradeInfo>();

    // 특정 Grade에 해당하는 GradeInfo를 반환
    public GradeInfo GetGradeInfo(string key, int grade)
    {
        return gradeInfoList.Find(info => info.unitKey == key && info.grade == grade);
    }
}