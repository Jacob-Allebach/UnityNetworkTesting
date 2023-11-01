using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerController : NetworkBehaviour
{

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<bool> host = new NetworkVariable<bool>(false);
    public NetworkVariable<int> health = new NetworkVariable<int>(3);
    [SerializeField]
    public GameObject orbPrefab;
    [SerializeField]
    public GameObject healthTag;
    public int cooldown = 0;

    private void Start()
    {
        if (NetworkManager.Singleton.IsHost) NetworkManager.Singleton.SpawnManager.GetLocalPlayerObject().GetComponent<PlayerController>().host.Value = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner == true)
        {

            Vector3 movement = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.W)) movement.z = +1f;
            if (Input.GetKey(KeyCode.S)) movement.z = -1f;
            if (Input.GetKey(KeyCode.A)) movement.x = -1f;
            if (Input.GetKey(KeyCode.D)) movement.x = +1f;

            float moveSpeed = 3f;

            MovementServerRpc(movement, moveSpeed);

            if (cooldown < 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OrbSpawnServerRpc(new Vector3(-1, 0, 0));
                    cooldown = 100;
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OrbSpawnServerRpc(new Vector3(1, 0, 0));
                    cooldown = 100;
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    OrbSpawnServerRpc(new Vector3(0, 0, 1));
                    cooldown = 100;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    OrbSpawnServerRpc(new Vector3(0, 0, -1));
                    cooldown = 100;
                }
            }
        }

        healthTag.GetComponent<TextMeshPro>().SetText($"{health.Value}");
        cooldown -= 1;
        transform.position = Position.Value;
    }

    public void takeDamage()
    {
        health.Value--;
    }

    [ServerRpc]
    void MovementServerRpc(Vector3 movement, float moveSpeed, ServerRpcParams rpcParams = default)
    {
        Position.Value += movement * moveSpeed * Time.deltaTime;
    }

    [ServerRpc]
    void SetHostServerRpc()
    {
        host.Value = true;
    }

    [ServerRpc]
    void OrbSpawnServerRpc(Vector3 direction)
    {
        GameObject newOrb = Instantiate(orbPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z) + direction, Quaternion.identity);
        newOrb.GetComponent<NetworkObject>().Spawn();
        newOrb.GetComponent<OrbBehavior>().SetVelocity(direction);
    }
}
