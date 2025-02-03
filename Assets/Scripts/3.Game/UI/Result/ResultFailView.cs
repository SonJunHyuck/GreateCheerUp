using UnityEngine;

public class ResultFailView : MonoBehaviour
{
    [SerializeField] private Animator animatorAlly;

    private void OnEnable() 
    {
        animatorAlly.SetBool("IsDead", true);
    }
}
