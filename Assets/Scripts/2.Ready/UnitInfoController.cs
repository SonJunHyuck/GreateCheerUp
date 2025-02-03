using System;
using UnityEngine;

// Observer -> Controller -> View
public class UnitInfoController : MonoBehaviour
{
    [SerializeField] private UnitInfoView unitInfoView; // View
    
    private string selectedUnitKey = "";

    private void OnEnable()
    {
        Observer.Instance.OnRequestUnitInfo += ResponseUnitInfo;
        Observer.Instance.OnRequestUpgradeUnit += ResponseUpgradeUnit;
    }

    private void OnDisable()
    {
        Observer.Instance.OnRequestUnitInfo -= ResponseUnitInfo;
        Observer.Instance.OnRequestUpgradeUnit -= ResponseUpgradeUnit;
    }
    
    // Observer로부터 이벤트를 받아 Model과 소통 후, View 업데이트 호출
    private void ResponseUnitInfo(string unitKey, bool forced = false)
    {
        if(!forced && selectedUnitKey == unitKey)
        {
            DebugWrapper.LogWarning("동일 유닛을 선택했습니다.");
            return;
        }

        selectedUnitKey = unitKey;

        // Model에서 데이터 읽기
        AllyInfoScriptableObject.UnitInfo unitInfo = DataManager.Instance.GetUnitInfo(unitKey);

        int currentGrade = PlayerDataManager.GetUnitGrade(unitKey);
        UnitGradeScriptableObject.GradeInfo gradeInfo = DataManager.Instance.GetGradeInfo(unitKey, currentGrade);

        
        UnitGradeScriptableObject.GradeInfo nextGradeInfo;
        if(currentGrade == DataManager.Instance.MaxUnitGrade)
        {
            nextGradeInfo = null;
        }
        else
        {
            int nextGrade = Math.Min(currentGrade + 1, DataManager.Instance.MaxUnitGrade);
            nextGradeInfo = DataManager.Instance.GetGradeInfo(unitKey, nextGrade);
        }
        
        int playerCurrency = PlayerDataManager.GetCurrency();

        // View 업데이트
        unitInfoView.UpdateUnitDetailView(unitInfo, gradeInfo, nextGradeInfo, playerCurrency);
    }

    private void ResponseUpgradeUnit()
    {
        if(string.IsNullOrEmpty(selectedUnitKey))
        {
            Debug.Log("선택한 유닛이 없습니다.");
            return;
        }

        // 업그레이드에 필요한 재화 <= 현재 재화
        int playerGold = PlayerDataManager.GetCurrency();
        int selectedUnitGrade = PlayerDataManager.GetUnitGrade(selectedUnitKey);
        int requiredGold = DataManager.Instance.GetGradeInfo(selectedUnitKey, selectedUnitGrade).upgradeGold;
        
        if(playerGold >= requiredGold)
        {
            if(PlayerDataManager.SpendCurrency(requiredGold))
            {
                // 유닛 업그레이드
                PlayerDataManager.UpgradeUnit(selectedUnitKey);
                Observer.Instance.RequestNoticeMessage(NoticeMessageView.MessageType.SuccessUpgrade);
                AudioManager.Instance.PlaySFX(AudioManager.Sfx.Success);

                // 유닛 정보 뷰 업데이트
                unitInfoView.UpdateCurrency();
                ResponseUnitInfo(selectedUnitKey, true);
            }
            else
            {
                DebugWrapper.Log("재화 사용 중 오류가 발생하였습니다.");
            }
        }
        else
        {
            Observer.Instance.RequestNoticeMessage(NoticeMessageView.MessageType.NeedMoreGold);
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Error);
            // 실패
            // unitInfoVIew.Alert();  // 재화가 부족합니다.
            // Alert의 경우에는 특정 컨버스를 하나 만들어서 동작시키는 것이 나을듯
            // DebugWrapper.Log("재화가 부족합니다.");
        }

    }
}
