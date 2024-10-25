using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] float hurtTime = 0.1f;
    [SerializeField] Enemy enemy;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    private float currentHealth;

    void OnValidate()
    {
        if (enemy == null)
            enemy = GetComponent<Enemy>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHealth = enemy.EnemyStats.Health;
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(ApplyHurtEffect());
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator ApplyHurtEffect()
    {
        animator.SetTrigger("OnHurt");
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(hurtTime);
        spriteRenderer.color = Color.white;
    }
}