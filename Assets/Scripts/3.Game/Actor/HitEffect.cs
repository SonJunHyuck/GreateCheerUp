using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private SpriteRenderer[] spriteRenderers;
    private MaterialPropertyBlock propBlock;
    private WaitForSeconds flashDuration;

    void Start()
    {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        propBlock = new MaterialPropertyBlock();
        flashDuration = new WaitForSeconds(0.1f);
    }

    public void Flash()
    {
        StopAllCoroutines();

        foreach (var spriteRenderer in spriteRenderers)
        {
            // MaterialPropertyBlock을 통해 각 스프라이트의 속성을 개별적으로 설정
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 1);
            spriteRenderer.SetPropertyBlock(propBlock);

            StartCoroutine(ResetFlashEffect(spriteRenderer));
        }
    }

    public void Flash(float duration)
    {
        StopAllCoroutines();

        foreach (var spriteRenderer in spriteRenderers)
        {
            // MaterialPropertyBlock을 통해 각 스프라이트의 속성을 개별적으로 설정
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 1);
            spriteRenderer.SetPropertyBlock(propBlock);

            StartCoroutine(ResetFlashEffect(duration));
        }
    }

    public void Flash(float duration, Color color)
    {
        StopAllCoroutines();
        
        foreach (var spriteRenderer in spriteRenderers)
        {
            // MaterialPropertyBlock을 통해 각 스프라이트의 속성을 개별적으로 설정
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 0.75f);
            propBlock.SetColor("_FlashColor", color);
            spriteRenderer.SetPropertyBlock(propBlock);

            StartCoroutine(ResetFlashEffect(duration));
        }
    }

    private IEnumerator ResetFlashEffect(SpriteRenderer spriteRenderer)
    {
        yield return flashDuration;

        // _FlashAmount 값을 원래 상태로 복귀
        spriteRenderer.GetPropertyBlock(propBlock);
        propBlock.SetFloat("_FlashAmount", 0);
        spriteRenderer.SetPropertyBlock(propBlock);
    }

    private IEnumerator ResetFlashEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        foreach (var spriteRenderer in spriteRenderers)
        {
            // MaterialPropertyBlock을 통해 각 스프라이트의 속성을 개별적으로 설정
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 0);
            spriteRenderer.SetPropertyBlock(propBlock);
        }
    }

    public void Reset()
    {
        StopAllCoroutines();

        foreach (var spriteRenderer in spriteRenderers)
        {
            // MaterialPropertyBlock을 통해 각 스프라이트의 속성을 개별적으로 설정
            spriteRenderer.GetPropertyBlock(propBlock);
            propBlock.SetFloat("_FlashAmount", 0);
            spriteRenderer.SetPropertyBlock(propBlock);
        }
    }
}
