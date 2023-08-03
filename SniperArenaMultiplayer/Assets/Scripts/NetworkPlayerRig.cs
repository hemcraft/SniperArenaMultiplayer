using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayerRig : NetworkBehaviour
{
    public float speed;
    public NetworkObject networkObject;
    public Animator soldierAnimator;
    public GameObject soldierMesh;

    public LayerMask playerSoldier;
    public LayerMask enemySoldier;

    private CharacterController characterController;

    private bool running;

    public override void Spawned()
    {
        base.Spawned();

        characterController = GetComponent<CharacterController>();

        if (networkObject.HasStateAuthority)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0f, 0.635f, 0.12f);
            Camera.main.transform.localRotation = Quaternion.identity;

            soldierMesh.layer = LayerMask.NameToLayer("PlayerSoldier");
        }
        else
        {
            soldierMesh.layer = LayerMask.NameToLayer("EnemySoldier");
        }
    }

    private void Update()
    {
        if (networkObject.HasStateAuthority)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            characterController.Move(move * speed * Time.deltaTime);

            if (move == Vector3.zero)
            {
                if (running == true)
                {
                    running = false;

                    soldierAnimator.CrossFade("demo_combat_idle", 0.15f);
                }
            } 
            else
            {
                if (running == false)
                {
                    running = true;

                    soldierAnimator.CrossFade("demo_combat_run", 0.15f);
                }
            }

            //fix vertical position
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
    }
}
