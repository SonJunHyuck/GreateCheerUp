using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextPlayTimer : MonoBehaviour
{
    private Text textTime;
    private void OnEnable()
    {
        textTime = GetComponent<Text>();
        GameManager.Instance.onUpdatePlayTime += ResponsePlayTime;
    }
    private void OnDisable()
    {
        GameManager.Instance.onUpdatePlayTime -= ResponsePlayTime;
    }

    private void ResponsePlayTime(int time)
    {
        int min = time / 60;
        int sec = time % 60;

        min = Mathf.Clamp(min, 0, 99);
        sec = Mathf.Clamp(sec, 0, 99);
        textTime.text = $"PLAY TIME ({min:D2}:{sec:D2})"; 
    }
}
