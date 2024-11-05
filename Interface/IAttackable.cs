using System;
using UnityEngine;

public interface IAttackable
{
    public event Action<bool> OnStateChanged;

    public bool HasTarget { get; }

    public abstract void TryAttack(float direction);
    public abstract void Attack(float direction); // 공통 공격 메서드

}