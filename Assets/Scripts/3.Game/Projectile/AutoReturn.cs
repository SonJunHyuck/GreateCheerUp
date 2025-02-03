using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoReturn : MonoBehaviour
{
    public float delayTime = 3.0f;

    private void OnEnable() 
    {
        StartCoroutine(DelayedReturn());
    }

    IEnumerator DelayedReturn()
    {
        yield return new WaitForSeconds(delayTime);

        gameObject.SetActive(false);
    }
}
