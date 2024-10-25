using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;


[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IKnockable
{
    public readonly float GRAVITY = 9.81f;
    [SerializeField] EnemyStats enemyStats;
    [SerializeField] Controller2D controller2D;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Animator animator;
    public Controller2D Controller2D => controller2D;
    public EnemyStats EnemyStats => enemyStats;

    Vector2 velocity;
    float forceOfGravity;

    void OnValidate()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (controller2D == null)
            controller2D = GetComponent<Controller2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        forceOfGravity = GRAVITY * enemyStats.Mass;
    }

    void FixedUpdate()
    {
        velocity.y -= forceOfGravity;

        controller2D.Move(velocity, false);
    }

    public void OnKnock(float force, int direction)
    {
        velocity.x = force * direction;
        velocity.y = Mathf.Abs(force * direction);
    }
}