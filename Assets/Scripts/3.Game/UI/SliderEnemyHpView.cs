using UnityEngine;

public class SliderEnemyHpView : SliderBaseHpView
{
    public override void OnEnable()
    {
        UnitBossEnemyHealth enemyHealth = GameObject.FindObjectOfType<UnitBossEnemyHealth>();
        if(enemyHealth != null)
        {
            enemyHealth.onSetHp += ResponseSetHp;
            enemyHealth.SetHpView();
        }
    }

    public override void OnDisable()
    {
        UnitBossEnemyHealth enemyHealth = GameObject.FindObjectOfType<UnitBossEnemyHealth>();
        if(enemyHealth != null)
        {
            enemyHealth.onSetHp -= ResponseSetHp;
        }
    }
}