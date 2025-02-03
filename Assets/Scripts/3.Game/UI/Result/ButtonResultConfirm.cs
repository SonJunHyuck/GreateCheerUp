using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonResultConfirm : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Resources.UnloadUnusedAssets();

            // LoadSceneManager.Instance.LoadScene("ReadyScene");
            SceneController.Instance.LoadScene(SceneKey.Ready);
            // SceneManager.LoadScene("ReadyScene", LoadSceneMode.Single);
        });
    }
}