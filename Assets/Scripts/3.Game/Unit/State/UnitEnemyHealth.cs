public class UnitEnemyHealth : UnitHealth
{
    public override void Die()
    {
        base.Die();

        string key = GetComponent<UnitAttributes>().key;

        int gold = DataManager.Instance.GetEnemyGold(key);

        GameManager.Instance.GainGold(gold);
    }
}