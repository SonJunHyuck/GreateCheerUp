using UnityEngine;
using UnityEngine.UI;

// Button -> Observer
[RequireComponent(typeof(Button))]
public class UnitUpgradeButton : MonoBehaviour
{
    private void OnEnable() 
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    // 버튼 클릭 이벤트
    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        Observer.Instance.RequestUpgradeUnit();
    }
}