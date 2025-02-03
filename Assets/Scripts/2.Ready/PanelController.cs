using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels; // 패널 리스트 (UnitUpgrade, StageSelect 등)

    private GameObject activePanel; // 현재 활성화된 패널

    private void Start()
    {
        if (panels.Count > 0)
        {
            // 초기 활성 패널 설정
            activePanel = panels[0];
            ActivatePanel(activePanel);
        }
        
    }

    private void OnEnable()
    {
        Observer.Instance.OnRequestPanelSwitch += SwitchPanel;
    }

    private void OnDisable()
    {
        Observer.Instance.OnRequestPanelSwitch -= SwitchPanel;
    }

    // 특정 패널 활성화 메서드
    public void ActivatePanel(GameObject panel)
    {
        foreach (var p in panels)
        {
            p.SetActive(false); // 모든 패널 비활성화
        }

        panel.SetActive(true); // 선택된 패널 활성화
        activePanel = panel;
    }

    // Observer에서 호출할 전환 메서드
    public void SwitchPanel(string panelName)
    {
        GameObject panel = panels.Find(p => p.name == panelName);
        if (panel != null)
        {
            ActivatePanel(panel);
        }
        else
        {
            Debug.LogWarning($"Panel '{panelName}' not found!");
        }
    }
}