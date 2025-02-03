public class UnitBossAllyHealth : UnitBossHealth
{
    protected override void Start()
    {
        base.Start();

        maxHealth = DataManager.Instance.GetCurrentStageInfo().AllyBossMaxHp;
        currentHealth = maxHealth;
        SetHpView();
    }

    public override void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        DebugWrapper.Log($"{gameObject.name}이(가) {damage}의 피해를 입었습니다. 현재 체력: {currentHealth}");

        SetHpView();

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하일 때 사망 처리

            // 게임 실패 처리
            GameManager.Instance.ClearStage(false);
        }

        if(!isDead)
        {
            hitEffect.Flash();
        }
    }
}
