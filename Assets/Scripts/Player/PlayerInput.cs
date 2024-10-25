using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement movement;
    [SerializeField] PlayerSlash playerSlash;

    void Update()
    {
        Vector2 directionalInput = new(Input.GetAxisRaw("Horizontal"), 0);
        movement.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space))
            movement.OnJumpInputDown();

        if (Input.GetKeyUp(KeyCode.Space))
            movement.OnJumpInputUp();

        if (Input.GetKeyDown(KeyCode.C))
            movement.OnDashInputDown();

        if (Input.GetKeyDown(KeyCode.E))
            playerSlash.OnSlashButtonDown();
    }
}
