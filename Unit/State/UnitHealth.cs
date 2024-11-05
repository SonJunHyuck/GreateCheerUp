using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealth : MonoBehaviour, IDamagable
{
    public float maxHealth = 50; // 적의 최대 체력
    [SerializeField] private float currentHealth;

    private bool isDead;
    public bool IsDead => isDead; // IsDead 프로퍼티

    private HitEffect hitEffect;

    private void OnEnable() 
    {
        isDead = false;
        currentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
    }

    void Start()
    {
        hitEffect = GetComponent<HitEffect>();
    }
    
    // IDamagable 인터페이스 구현: 데미지를 입는 처리
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        DebugWrapper.Log($"{gameObject.name}이(가) {damage}의 피해를 입었습니다. 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하일 때 사망 처리
        }

        if(!isDead)
        {
            hitEffect.Flash();
        }
    }

    // IDamagable 인터페이스 구현: 사망 처리
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        DebugWrapper.Log($"{gameObject.name}이(가) 사망했습니다.");

        // 사망 처리 (애니메이션, 적 제거 로직 등 추가 가능)
        StartCoroutine(DelayedDie());
    }

    IEnumerator DelayedDie()
    {
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsStun", false);
        animator.SetBool("IsDead", true);
        animator.speed = 1.0f;
        
        hitEffect.Reset();
        
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(3.0f);

        gameObject.SetActive(false);
    }
}
