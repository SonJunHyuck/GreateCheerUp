using System;

public interface IDamagable
{    
    void TakeDamage(float damage); // 데미지를 입는 메서드
    void Die(); // 사망 처리를 하는 메서드

    bool IsDead { get; } // 사망 여부를 나타내는 프로퍼티

}