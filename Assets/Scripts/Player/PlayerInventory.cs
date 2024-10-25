using Unity.VisualScripting;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeReference] public GameObject item1GO;
    IItem item1;
    void OnValidate()
    {
        if (!item1GO.TryGetComponent<IItem>(out item1))
        {
            item1GO = null;
        }
    }

    public void OnItemInputDown(PlayerMovement movement)
    {
        item1.Use(movement);
    }
}