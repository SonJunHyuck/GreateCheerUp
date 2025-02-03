using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonAllySpawn : MonoBehaviour
{
    [SerializeField] private Image iconImage; // 버튼에 표시될 아이콘 이미지
    [SerializeField] private Text textNumber;

    private string allyKey; // Ally의 Addressable Key

    // AllySpawnButton 초기화
    public void Initialize(string key, int cost, Sprite sprite, int num)
    {
        allyKey = key;
        gameObject.name = key;

        if(iconImage == null)
        {
            iconImage = transform.Find("Icon").GetComponent<Image>();
        }
        
        iconImage.sprite = sprite;
        iconImage.SetNativeSize();

        textNumber.text = num.ToString();

        GetComponent<Button>().onClick.AddListener(
            () => Observer.Instance.RequestSpawnAlly(allyKey, cost));
    }
}