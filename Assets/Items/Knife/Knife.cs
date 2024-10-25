using System;
using UnityEngine;

[Serializable]
public class Knife : MonoBehaviour, IItem
{
    [SerializeField] KnifeStats stats;
    [SerializeField] ParticleSystem particle;

    bool isOnCooldown;

    public void Use(PlayerMovement playerMovement)
    {
        if (isOnCooldown)
            return;

        isOnCooldown = true;
        Invoke(nameof(ResetCooldown), stats.cooldownTime);

        ParticleSystem spawnedParticle = Instantiate(particle, playerMovement.transform);
        spawnedParticle.transform.position = new Vector3(stats.startOffset.x * playerMovement.GetFaceDirection(), stats.startOffset.y);
        if (playerMovement.GetFaceDirection() > 0)
        {
            spawnedParticle.GetComponent<ParticleSystemRenderer>().flip = new Vector3(1, 0, 0);
        }

        RaycastHit2D[] hits = Physics2D.CircleCastAll(stats.startOffset, stats.extends.x, Vector2.zero);
        foreach (RaycastHit2D hit2D in hits)
        {
            if (hit2D.collider.CompareTag("Damageable"))
            {
                hit2D.collider.GetComponent<IDamageable>().Damage(stats.damage);
            }
        }
    }

    void OnDrawGizmoSelected()
    {
        Gizmos.DrawSphere(stats.startOffset, stats.extends.x);
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
    }
}