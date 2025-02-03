public class UnitBossEnemyHealth : UnitBossHealth
{
    protected override void Start()
    {
        base.Start();

        maxHealth = DataManager.Instance.GetCurrentStageInfo().EnemyBossMaxHp;
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

            // 스테이지 클리어 처리
            GameManager.Instance.ClearStage(true);
        }

        if(!isDead)
        {
            hitEffect.Flash();
        }
    }
}
