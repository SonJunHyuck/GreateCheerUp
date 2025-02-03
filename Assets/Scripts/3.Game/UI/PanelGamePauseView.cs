using UnityEngine;

public class PanelGamePauseView : MonoBehaviour 
{
    [SerializeField] private GameObject panelPause;

    private void Awake() 
    {
        if(panelPause == null)
        {
            panelPause = transform.GetChild(0).gameObject;
        }
    }

    private void OnEnable() 
    {
        GameManager.Instance.onPauseGame += ResponsePauseGame;
    }

    private void OnDisable() 
    {
        GameManager.Instance.onPauseGame -= ResponsePauseGame;
    }

    private void ResponsePauseGame(bool isActive)
    {
        panelPause.SetActive(isActive);
    }
}