using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StageData", menuName = "Game Data/Stage Data", order = 1)]
public class StageInfoScriptableObject : ScriptableObject
{
    [System.Serializable]
    public class StageInfo
    {
        public int stageId;          // 스테이지 ID
        public string stageName;     // 스테이지 이름
        public string sceneName;  // 씬 이름
        public string description;   // 스테이지 설명

        public int timeLimit1;      // 별 3개
        public int timeLimit2;      // 별 2개

        public int AllyBossMaxHp;
        public int EnemyBossMaxHp;
    }

    public List<StageInfo> stageInfoList; // 해당 스테이지에서의 유닛 스폰 규칙 리스트

    public StageInfo GetStageInfo(int stagdId)
    {
        return stageInfoList.Find(info => info.stageId == stagdId);
    }
}