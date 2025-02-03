using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonGameStart : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener( () => 
        {
            AudioManager.Instance.PlaySFX(AudioManager.Sfx.Button);
            AudioManager.Instance.PlayBGM(false);

            if(DataManager.Instance.IsDataLoaded)
            {
                // SceneManager.LoadScene("ReadyScene");
                // LoadSceneManager.Instance.LoadScene("ReadyScene");
                SceneController.Instance.LoadScene(SceneKey.Ready);
            }
            else
            {
                DebugWrapper.LogWarning("DataManager is not Ready yet");
            }
        } );
    }
}
