using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClose : MonoBehaviour
{
    [SerializeField] private GameObject rootPanel;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
            rootPanel.SetActive(false);
        });
    }
}
