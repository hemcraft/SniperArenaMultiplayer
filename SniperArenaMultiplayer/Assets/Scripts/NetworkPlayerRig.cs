using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkPlayerRig : NetworkBehaviour
{
    [Networked]
    public int Health { get; set; }
    [Networked]
    public int Score { get; set; }

    [Networked]
    public NetworkString<_64> PlayerEditorName { get; set; }

    [Networked]
    public NetworkString<_64> PlayerNickName { get; set; }

    public float speed;
    public NetworkObject networkObject;
    public Animator soldierAnimator;
    public GameObject soldierMesh;
    public GameObject weapon;
    public ParticleSystem explosionSystem;
    public Slider healthSlider;

    public LayerMask playerSoldier;
    public LayerMask enemySoldier;

    private AudioSource audioSource;
    private CharacterController characterController;
    private UserInterface userInterface;
    private SpawnManager spawnManager;

    private bool running;

    public override void Spawned()
    {
        base.Spawned();

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        userInterface = FindObjectOfType<UserInterface>();
        spawnManager = FindObjectOfType<SpawnManager>();

        if (networkObject.HasStateAuthority)
        {
            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0f, 0.635f, 0.12f);
            Camera.main.transform.localRotation = Quaternion.identity;

            soldierMesh.layer = LayerMask.NameToLayer("PlayerSoldier");
            weapon.layer = LayerMask.NameToLayer("EnemySoldier");

            Health = 100;
            healthSlider.value = Health;
            userInterface.healthValueText.text = Health.ToString();

            LoadPlayerNickName();
            userInterface.SetMainPlayerRig(this);
        }
        else
        {
            soldierMesh.layer = LayerMask.NameToLayer("EnemySoldier");
            weapon.layer = LayerMask.NameToLayer("PlayerSoldier");

            userInterface.SetEnemyPlayerRig(this);

            StartCoroutine(SetEditorNameInSeconds());
        }
    }

    private void Update()
    {
        if (networkObject.HasStateAuthority)
        {
            userInterface.healthValueText.text = Health.ToString();

            float horizontalAxis = Input.GetAxis("Horizontal");
            float verticalAxis = Input.GetAxis("Vertical");

            var forward = transform.forward;
            var right = transform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            var desiredMoveDirection = forward * verticalAxis + right * horizontalAxis;

            characterController.Move(desiredMoveDirection * speed * Time.deltaTime);

            if (desiredMoveDirection == Vector3.zero)
            {
                if (running == true)
                {
                    running = false;

                    RPCPlayAnimation("demo_combat_idle", 0.15f);
                }
            } 
            else
            {
                if (running == false)
                {
                    running = true;

                    RPCPlayAnimation("demo_combat_run", 0.15f);
                }
            }

            //fix vertical position
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        healthSlider.value = Health;
    }

    private void Shoot()
    {
        Debug.Log("Shoot");

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                NetworkPlayerRig enemyNetworkPlayerRig = hit.collider.GetComponent<NetworkPlayerRig>();
                enemyNetworkPlayerRig.HitFromBullet();
            }
        }

        explosionSystem.Play();
        RPCPlayAudio();
    }

    public void HitFromBullet()
    {
        RPCHitFromBullet();
    }

    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = true)]
    public void RPCHitFromBullet()
    {
        if (networkObject.HasStateAuthority)
        {
            Health = Health - 50;

            if (Health == 0)
            {
                Die();
            }
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = true)]
    public void RPCPlayAudio()
    {
        audioSource.Play();
    }

    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = true)]
    public void RPCPlayAnimation(string stateName, float normalizedTransitionDuration)
    {
        soldierAnimator.CrossFade(stateName, normalizedTransitionDuration);
    }

    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = false)]
    public void RPCSetName(string editorName)
    {
        gameObject.name = editorName;
    }

    public void SetEditorName()
    {
        PlayerEditorName = gameObject.name;
    }

    IEnumerator SetEditorNameInSeconds()
    {
        yield return new WaitForSeconds(2f);

        gameObject.name = PlayerEditorName.ToString();
    }

    private void LoadPlayerNickName()
    {
        PlayerNickName = "PLAYER" + networkObject.Id;
    }

    private void Die()
    {
        Health = 100;
        transform.position = spawnManager.GetSpawnPoint().position;

        userInterface.ShowRedPanel();

        userInterface.enemyPlayerRig.RPCPlayerScoreIncrease();
    }

    [Rpc(RpcSources.All, RpcTargets.All, InvokeLocal = false)]
    public void RPCPlayerScoreIncrease()
    {
        if (networkObject.HasStateAuthority)
        {
            Score = Score + 1;

            Debug.Log("Score Increase");
            userInterface.ShowGreenPanel();
        }
    }
}
