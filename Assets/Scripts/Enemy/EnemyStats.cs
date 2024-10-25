using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy", fileName = "New Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    [SerializeField] string enemyName;
    [SerializeField] int damage;
    [SerializeField] int health;
    [SerializeField] float speed;
    [SerializeField] float mass;

    public string EnemyName => enemyName;
    public int Damage => damage;
    public int Health => health;
    public float Speed => speed;
    public float Mass => mass;
}