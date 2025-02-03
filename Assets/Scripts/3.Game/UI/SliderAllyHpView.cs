using UnityEngine;

public class SliderAllyHpView : SliderBaseHpView
{
    public override void OnEnable()
    {
        UnitBossAllyHealth allyHealth = GameObject.FindObjectOfType<UnitBossAllyHealth>();
        if(allyHealth != null)
        {
            allyHealth.onSetHp += ResponseSetHp;
            allyHealth.SetHpView();
        }
    }

    public override void OnDisable()
    {
        UnitBossAllyHealth allyHealth = GameObject.FindObjectOfType<UnitBossAllyHealth>();
        if(allyHealth != null)
        {
            allyHealth.onSetHp -= ResponseSetHp;
        }
    }
}