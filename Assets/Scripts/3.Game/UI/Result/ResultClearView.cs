using UnityEngine;

public class ClearResultView : MonoBehaviour
{
    [SerializeField] private Animator animatorAlly;
    [SerializeField] private Animator animatorEnemy;

    private void OnEnable() 
    {
        animatorAlly.SetTrigger("DoAttack");
        animatorEnemy.SetBool("IsDead", true);    
    }
}
