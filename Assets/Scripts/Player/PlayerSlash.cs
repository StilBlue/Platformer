using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] float damage = 3;
    [SerializeField] Animator animator;
    [SerializeField] AnimationClip slashAnimation;
    [SerializeField] float slashCooldown = 0.1f;

    float slashTime;

    bool isOnCooldown;
    readonly List<GameObject> collidingDamageable = new();

    void Start()
    {
        slashTime = slashAnimation.length;
        Debug.Log(slashTime);
    }

    public void OnSlashButtonDown()
    {
        if (isOnCooldown || !playerMovement.Controller2D.collisions.below)
            return;


        animator.SetTrigger("OnAttack");

        StartCoroutine(RunSlashTime());
        StartCoroutine(RunCooldown());

        foreach (GameObject damageableGO in collidingDamageable.ToList())
        {
            damageableGO.GetComponent<IDamageable>().Damage(damage);
            if (damageableGO.TryGetComponent(out IKnockable knockable))
            {
                knockable.OnKnock(damage, (int)Mathf.Sign(damageableGO.transform.position.x - transform.position.x));
            }
        }
    }

    IEnumerator RunSlashTime()
    {
        playerMovement.isSlashing = true;
        yield return new WaitForSecondsRealtime(slashTime);
        playerMovement.isSlashing = false;
    }

    IEnumerator RunCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(slashCooldown);
        isOnCooldown = false;
    }

    void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Damageable"))
        {
            collidingDamageable.Add(collision2D.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D collision2D)
    {
        collidingDamageable.Remove(collision2D.gameObject);
    }
}