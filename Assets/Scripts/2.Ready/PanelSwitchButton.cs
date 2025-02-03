using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class PanelSwitchButton : MonoBehaviour
{
    [SerializeField] private string targetPanelName; // 이동할 패널 이름

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
        Observer.Instance.RequestPanelSwitch(targetPanelName);
    }
}