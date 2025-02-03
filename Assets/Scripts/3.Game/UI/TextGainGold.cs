using UnityEngine;
using UnityEngine.UI;

public class TextGainGold : MonoBehaviour
{
    private Text textGainGold;

    private void Awake() 
    {
        textGainGold = GetComponent<Text>();    
    }

    private void OnEnable() 
    {   
        GameManager.Instance.onUpdateGainGold += ResponseGainGold;    
    }

    private void OnDisable() 
    {
        GameManager.Instance.onUpdateGainGold -= ResponseGainGold;    
    }

    private void ResponseGainGold(int totalGold)
    {
        textGainGold.text = totalGold.ToString("N0");
    }
}