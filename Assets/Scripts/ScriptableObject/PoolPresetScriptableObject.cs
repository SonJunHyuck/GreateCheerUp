using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PoolPreset", menuName = "ScriptableObjects/PoolPreset", order = 3)]
public class PoolPresetScriptableObject : ScriptableObject
{
    // AllyInfo 구조체를 사용하여 id와 tag를 함께 관리
    [System.Serializable]
    public struct PoolPresetInfo
    {
        public string key;
        public int size;  // 미리 생성해 둘 오브젝트 수
    }

    // Info 리스트로 관리
    public List<PoolPresetInfo> poolPresetInfoList = new List<PoolPresetInfo>();
}