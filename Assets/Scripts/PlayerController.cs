using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<bool> host = new NetworkVariable<bool>(false);
    [SerializeField]
    public GameObject orbPrefab;

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

            if (Input.GetKeyDown(KeyCode.O)) OrbSpawnServerRpc();
        }

        transform.position = Position.Value;
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
    void OrbSpawnServerRpc()
    {
        GameObject newOrb = Instantiate(orbPrefab, new Vector3(transform.position.x, 1, transform.position.z), Quaternion.identity);
        newOrb.GetComponent<NetworkObject>().Spawn();
    }
}
