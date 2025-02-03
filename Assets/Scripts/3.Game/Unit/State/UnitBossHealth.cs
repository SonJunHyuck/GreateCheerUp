using System;

// Model + Controller
public class UnitBossHealth : UnitHealth
{
    public event Action<float, float> onSetHp;

    public void SetHpView()
    {
        onSetHp?.Invoke(maxHealth, currentHealth);
    }
}
