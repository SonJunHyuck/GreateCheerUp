using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class ButtonPause : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            GameManager.Instance.HandlePauseGame();
        }
        );
    }

    private void OnDisable()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }
}