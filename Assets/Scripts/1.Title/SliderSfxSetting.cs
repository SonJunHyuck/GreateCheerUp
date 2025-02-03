using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderSfxSetting : MonoBehaviour
{
     void OnEnable() 
     {
        GetComponent<Slider>().value = ConfigDataManager.GetSfxVolume;
     }

    void Start()
    {
        GetComponent<Slider>().onValueChanged.AddListener( value =>
        {
            AudioManager.Instance.SFXVolume = value;
        });
    }
}