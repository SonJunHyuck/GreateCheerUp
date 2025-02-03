using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonGameQuit : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener( () => 
        {
            DebugWrapper.Log("Quit Game");
            Application.Quit();
        } );
    }
}
