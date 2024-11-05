using System.Collections;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public float maxHealth = 100; // 최대 체력
    private float currentHealth;
    private bool isDead = false;
    public bool IsDead => isDead; // IsDead 프로퍼티

    void Start()
    {
        currentHealth = maxHealth; // 초기 체력을 최대 체력으로 설정
    }

    // IDamagable 인터페이스 구현: 데미지를 입는 처리
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"플레이어가 {damage}의 피해를 입었습니다. 현재 체력: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하일 때 사망 처리
        }
    }

    public void TakeDamageWithHitEffect(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        DebugWrapper.Log($"{gameObject.name}이(가) {damage}의 피해를 입었습니다. 현재 체력: {currentHealth}");

        // hitEffect.Flash();

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하일 때 사망 처리
        }
    }

    // IDamagable 인터페이스 구현: 사망 처리
    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("플레이어가 사망했습니다.");

        GetComponent<BoxCollider2D>().enabled = false;  // 다음 개체를 찾을 수 있도록 Collider를 꺼줌

        StartCoroutine(DelaiedDie());

        // 사망 처리 (애니메이션, 게임 오버 로직 등 추가 가능)
        // gameObject.SetActive(false); // 임시로 비활성화
    }

    IEnumerator DelaiedDie()
    {
        GetComponentInChildren<Animator>().SetBool("IsDead", true);
        yield return new WaitForSeconds(3.0f);

        // 사망 처리 (애니메이션, 게임 오버 로직 등 추가 가능)
        gameObject.SetActive(false); // 임시로 비활성화
        
    }

    // 체력을 회복하는 메서드
    public void Heal(float amount)
    {
        if(isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // 체력은 최대 체력을 넘지 않도록 제한
        }
        Debug.Log($"플레이어가 {amount}의 체력을 회복했습니다. 현재 체력: {currentHealth}");
    }
}