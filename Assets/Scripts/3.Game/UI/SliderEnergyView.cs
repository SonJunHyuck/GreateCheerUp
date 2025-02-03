using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderEnergyView : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI energyText;
    
    private void Awake() 
    {
        slider = gameObject.GetComponent<Slider>();
        energyText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }


    public void OnEnable()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        if(playerController != null)
        {
            playerController.onSetEnergy += ResponseSetEnergy;
            playerController.SetEnergy();
        }
    }

    public void OnDisable()
    {
        PlayerController playerController = GameObject.FindObjectOfType<PlayerController>();
        if(playerController != null)
        {
            playerController.onSetEnergy -= ResponseSetEnergy;
        }
    }

    private void ResponseSetEnergy(float maxEnergy, float currentEnergy)
    {
        slider.value = currentEnergy / maxEnergy;
        energyText.text = $"{currentEnergy:F0} / {maxEnergy:F0}";  // 부동소수점 표시 x
    }
}