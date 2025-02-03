using TMPro;
using UnityEngine;
using UnityEngine.UI;

// view
public abstract class SliderBaseHpView : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI healthText;

    private void Awake() 
    {
        slider = gameObject.GetComponent<Slider>();
        healthText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Controller 구독 구현
    public abstract void OnEnable();

    // Controller 구독 해제 구현
    public abstract void OnDisable();

    public void ResponseSetHp(float maxHealth, float currentHealth)
    {
        slider.value = currentHealth / maxHealth;
        healthText.text = $"{currentHealth} / {maxHealth}";
    }
}
