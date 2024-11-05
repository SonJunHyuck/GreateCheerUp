using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AddressableAsset", menuName = "ScriptableObjects/AddressableAsset", order = 1)]
public class AddressableAssetScriptableObject : ScriptableObject
{
    // AllyInfo 구조체를 사용하여 id와 tag를 함께 관리
    [System.Serializable]
    public struct AddressableAssetInfo
    {
        public string addressableKey;  // Asset Key : Load에 사용
        public string tag;  // Object Tag : 게임 내부에서 사용
    }

    // Info 리스트로 관리
    public List<AddressableAssetInfo> addressableAssetInfoList = new List<AddressableAssetInfo>();
}