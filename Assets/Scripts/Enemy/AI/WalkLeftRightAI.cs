using UnityEngine;

public class WalkLeftRightAI : ScriptableObject, IEnemyAIGoal
{
    float leftX;
    float rightX;

    public bool OnTrigger(Enemy enemy)
    {
        // Add walking AI here;
        return true;
    }
}
