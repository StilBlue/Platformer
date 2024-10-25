using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Damage(float damage)
    {
        animator.SetTrigger("OnHurt");
    }
}