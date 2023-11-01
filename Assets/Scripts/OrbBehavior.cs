using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OrbBehavior : NetworkBehaviour
{
    public NetworkVariable<Vector3> orbPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<Vector3> velocity = new NetworkVariable<Vector3>(new Vector3(0,0,0));
    public int orbSpeed = 5;
    public int timer = 0;

    private void Start()
    {
        orbPosition.Value = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        orbPosition.Value += velocity.Value * orbSpeed * Time.deltaTime;
        timer += 1;

        if(timer >= 600)
        {
            Destroy(this.gameObject);
        }

        transform.position = orbPosition.Value;
    }

    public void SetVelocity(Vector3 direction)
    {
        velocity.Value = direction;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().takeDamage();
            Destroy(this.gameObject);
        }
    }
}
