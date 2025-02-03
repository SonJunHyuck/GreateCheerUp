using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageView : MonoBehaviour
{
    public GameObject buttonPrefab; // 버튼 프리팹
    public Transform buttonContainer; // 버튼을 배치할 부모 오브젝트
    private List<GameObject> stageButtons = new List<GameObject>(); // 재사용 버튼 리스트

    [SerializeField] private Button previousButton;         // 이전 페이지 버튼
    [SerializeField] private Button nextButton;             // 다음 페이지 버튼
    [SerializeField] private Text pageText;      // 페이지 텍스트

    private int currentPage = 0; // 현재 페이지 (0부터 시작)
    private const int ButtonsPerPage = 10; // 페이지당 버튼 개수

    private void Start()
    {
        InitializeButtons(); // 버튼 초기화
        UpdatePage(0);        // 첫 페이지 로드
    }


    private void InitializeButtons()
    {
        for (int i = 0; i < ButtonsPerPage; i++)
        {
            GameObject button = Instantiate(buttonPrefab, buttonContainer);
            button.SetActive(true); // 버튼 활성화
            stageButtons.Add(button);
        }
    }

    public void UpdatePage(int increase)
    {
        int stageInfoListCount = DataManager.Instance.GetStageInfoListCount();  // Model에서 Get
        int goPage = currentPage + increase;

        // 다음 페이지 호출 시, 다음 페이지가 없을 경우
        if (goPage * ButtonsPerPage >= stageInfoListCount)
            return;

        // 이전 페이지 호출 시, 이전 페이지가 없을 경우
        if (goPage < 0)
            return;

        currentPage = goPage;

        int startIndex = currentPage * ButtonsPerPage + 1;
        int endIndex = Mathf.Min(startIndex + ButtonsPerPage, stageInfoListCount);

        for (int i = 0; i < stageButtons.Count; i++)
        {
            if (startIndex + i < endIndex)
            {
                StageInfoScriptableObject.StageInfo stage = DataManager.Instance.GetStageInfo(startIndex + i);  // Model에서 Get
                StageButton stageButton = stageButtons[i].GetComponent<StageButton>();

                // 스테이지 버튼 초기화
                int clearGrade = PlayerDataManager.GetStageClearGrade(stage.stageId);
                bool isOpen = PlayerDataManager.IsOpenStage(stage.stageId);

                stageButton.Initialize(stage.stageId, stage.stageName, stage.sceneName, clearGrade, isOpen);

                stageButtons[i].SetActive(true);
            }
            else
            {
                stageButtons[i].SetActive(false); // 버튼 숨김
            }
        }

        // 페이지 버튼 활성화/비활성화
        previousButton.interactable = currentPage > 0;
        nextButton.interactable = endIndex < stageInfoListCount;
        pageText.text = (currentPage + 1) + " / " + ((stageButtons.Count / ButtonsPerPage) + 1);
    }
}