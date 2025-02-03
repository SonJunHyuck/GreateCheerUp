using UnityEngine;
using UnityEngine.UI;

public class StageResultView : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject popupClear;
    [SerializeField] private GameObject popupFail;

    [SerializeField] private Transform starParent;

    [SerializeField] private Text textTime;
    [SerializeField] private Text textGold;

    private void OnEnable() 
    {
        Observer.Instance.onStageResultPopup += UpdatePopupView;
    }

    private void OnDisable() 
    {
        Observer.Instance.onStageResultPopup -= UpdatePopupView;
    }

    private void UpdatePopupView(bool isClear, int clearTime, int gainGold, int clearGrade)
    {
        // Open popup
        popup.SetActive(true);

        // Clear Popup
        popupClear.gameObject.SetActive(isClear);
        popupFail.gameObject.SetActive(!isClear);

        // Star
        for(int i = 0; i < clearGrade; i++)
        {
            starParent.GetChild(i).GetComponent<Image>().color = Color.white;
        }

        // Time
        int min = clearTime / 60;
        int sec = clearTime % 60;

        min = Mathf.Clamp(min, 0, 99);
        sec = Mathf.Clamp(sec, 0, 99);
        textTime.text = $"CLEAR TIME ({min:D2}:{sec:D2})"; 

        // Gold
        textGold.text = gainGold.ToString("N0");
        
    }
}