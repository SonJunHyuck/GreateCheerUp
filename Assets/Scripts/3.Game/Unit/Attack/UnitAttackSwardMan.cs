using System.Collections;

public class UnitAttackSwardMan : UnitAttackMelee
{
    protected override sealed IEnumerator SendDamage(IDamagable damagable, float direction)
    {
        yield return attackDelay;

        if(!damagable.IsDead)
        {
            damagable.TakeDamage(attackDamage);
        }
    }
}
