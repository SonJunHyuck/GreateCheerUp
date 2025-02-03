using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadySceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.ChangeBGM(AudioManager.Bgm.Ready);   
    }
}
