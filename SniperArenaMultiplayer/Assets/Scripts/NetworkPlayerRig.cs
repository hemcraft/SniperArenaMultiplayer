using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerRig : NetworkBehaviour
{
    public float speed;
    public NetworkObject networkObject;

    private CharacterController characterController;

    public override void Spawned()
    {
        base.Spawned();

        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (networkObject.HasStateAuthority)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            characterController.Move(move * speed * Time.deltaTime);
        }
    }
}
