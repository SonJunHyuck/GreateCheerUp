using UnityEngine;
using UnityEngine.UI;

// 데이터를 받아서 유닛 정보 출력 패널 출력 기능
public class UnitInfoView : MonoBehaviour
{
    [Header("Portrait")]
    [SerializeField] private Transform unitDisplay;
    [SerializeField] private Image unitIconImage;
    [SerializeField] private Text unitNameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text gradeText;

    private GameObject currentUnitObject;

    [Header("Status")]
    private UnitGradeScriptableObject.GradeInfo maxGradeInfo;
    [SerializeField] private Slider statusAttackSlider;
    [SerializeField] private Slider statusSpeedSlider;
    [SerializeField] private Slider statusHealthSlider;

    [Header("Upgrade")]
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text upgradeRequiredText;
    

    [Header("Currency")]
    [SerializeField] private Text currencyText;

    private void Start()
    {
        Initialize();
        UpdateCurrency();
    }

    private void Initialize()
    {
        maxGradeInfo = DataManager.Instance.GetMaxGradeInfo();
    }

    // Panel 업데이트
    public void UpdateUnitDetailView(AllyInfoScriptableObject.UnitInfo unitInfo, 
                        UnitGradeScriptableObject.GradeInfo gradeInfo, UnitGradeScriptableObject.GradeInfo nextGradeInfo,
                        int playerCurrency)
    {
        if (unitInfo == null)
        {
            Debug.LogWarning("No data to display!");
            return;
        }

        // 이전에 클릭했던 유닛 오브젝트 제거
        if(currentUnitObject != null)
        {
            DestroyImmediate(currentUnitObject);
        }

        currentUnitObject = Instantiate(unitInfo.unitUIPrefab, unitDisplay);
        currentUnitObject.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        unitNameText.text = unitInfo.unitName;
        unitIconImage.sprite = unitInfo.unitIcon;
        descriptionText.text = unitInfo.description;
        gradeText.text = $"{ gradeInfo.grade } / { DataManager.Instance.MaxUnitGrade }";

        statusAttackSlider.value = gradeInfo.attack / maxGradeInfo.attack;
        statusSpeedSlider.value = gradeInfo.speed / maxGradeInfo.speed;
        statusHealthSlider.value = gradeInfo.health / maxGradeInfo.health;

        bool isUnitMaxGrade = nextGradeInfo == null;

        // 유닛 버튼을 누를 때마다 갱신하기 때문에, 비용이 크지만 굳이 저장하지 않음
        if(isUnitMaxGrade)
        {
            statusAttackSlider.transform.Find("TextValue").GetComponent<Text>().text = "";
            statusSpeedSlider.transform.Find("TextValue").GetComponent<Text>().text = "";
            statusHealthSlider.transform.Find("TextValue").GetComponent<Text>().text = "";
        }
        else
        {
            float diff = nextGradeInfo.attack - gradeInfo.attack;
            string diffText = $"{gradeInfo.attack} (+{diff})";
            statusAttackSlider.transform.Find("TextValue").GetComponent<Text>().text = diffText;

            diff = nextGradeInfo.speed - gradeInfo.speed;
            diffText = $"{gradeInfo.speed} (+{diff})";
            statusSpeedSlider.transform.Find("TextValue").GetComponent<Text>().text = diffText;

            diff = nextGradeInfo.health - gradeInfo.health;
            diffText = $"{gradeInfo.health} (+{diff})";
            statusHealthSlider.transform.Find("TextValue").GetComponent<Text>().text = diffText;
        }

        if(isUnitMaxGrade)
        {
            upgradeButton.gameObject.SetActive(false);
            upgradeRequiredText.gameObject.SetActive(false);
        }
        else
        {
            upgradeButton.gameObject.SetActive(true);
            upgradeRequiredText.gameObject.SetActive(true);
            upgradeRequiredText.text = gradeInfo.upgradeGold.ToString();
            upgradeRequiredText.color = gradeInfo.upgradeGold > playerCurrency ? Color.red : Color.white;
        }

        
    }

    public void UpdateCurrency()
    {
        // Debug.Log(PlayerDataManager.GetCurrency());
        currencyText.text = PlayerDataManager.GetCurrency().ToString("N0");
    }
}