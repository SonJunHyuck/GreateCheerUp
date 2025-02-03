using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> characters;

    private void Start()
    {
        foreach(GameObject character in characters)
        {
            character.GetComponentInChildren<Animator>().SetBool("IsMoving", true);
        }

        AudioManager.Instance.SFXVolume = ConfigDataManager.GetSfxVolume;
        AudioManager.Instance.BGMVolume = ConfigDataManager.GetBgmVolume;
        AudioManager.Instance.ChangeBGM(AudioManager.Bgm.Title);
    }
}
