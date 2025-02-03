using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SettingButton : MonoBehaviour
{
    [SerializeField] private GameObject panelSetting;
    
    void Start()
    {
        GetComponent<Button>().onClick.AddListener( () => 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
            panelSetting.SetActive(true);
        });
    }
}