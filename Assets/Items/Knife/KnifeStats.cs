using UnityEngine;

[CreateAssetMenu(fileName = "New Knife Item", menuName = "Item/Knife")]
public class KnifeStats : ScriptableObject
{
    public string knifeName;
    public float damage;
    public Vector2 extends;
    public Vector2 startOffset;
    public float cooldownTime;
}