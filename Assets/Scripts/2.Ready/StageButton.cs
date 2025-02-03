using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StageButton : MonoBehaviour
{
    public GameObject lockImg;
    public GameObject starContainer;
    public TextMeshProUGUI stageNameText;       // 스테이지 이름 표시

    [SerializeField] private int stageId;
    private string sceneName;

    public void Initialize(int stageId, string stageName, string sceneName, int clearGrade, bool isOpen)
    {
        this.stageId = stageId;
        this.sceneName = sceneName;
        
        if(isOpen)
        {
            lockImg.SetActive(false);
            stageNameText.text = stageName;
            stageNameText.color = Color.green;
            stageNameText.gameObject.SetActive(true);
            GetComponent<Button>().interactable = true;
        }
        else
        {
            lockImg.SetActive(true);
            stageNameText.gameObject.SetActive(false);
            GetComponent<Button>().interactable = false;
        }
        for(int i = 0 ; i < 3; i++)
        {
            starContainer.transform.GetChild(i).GetComponent<Image>().color = Color.black;
        }

        if(clearGrade > 0)
        {
            stageNameText.color = Color.green;
        }

        for(int i = 0 ; i < clearGrade; i++)
        {
            starContainer.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }

        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        AudioManager.Instance.PlayBGM(false);
        Observer.Instance.RequestStageSelect(stageId, sceneName);
        Debug.Log(DataManager.Instance.GetStageInfo(stageId).timeLimit1);
    }
}