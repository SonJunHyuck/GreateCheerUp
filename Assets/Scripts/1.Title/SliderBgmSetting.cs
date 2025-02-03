using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderBgmSetting : MonoBehaviour
{
    void OnEnable() 
     {
        GetComponent<Slider>().value = ConfigDataManager.GetBgmVolume;
     }

    void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener( value =>
        {
            AudioManager.Instance.BGMVolume = value;
        });
    }
}