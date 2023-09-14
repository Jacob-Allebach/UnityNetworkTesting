using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalMovement = 0f;
    private float _verticalMovement = 0f;
    // Update is called once per frame
    void Update()
    {
        _horizontalMovement = Input.GetAxis("Horizontal");
        _verticalMovement = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(_horizontalMovement * 5, _verticalMovement * 5, 0);
        transform.Translate(movement * Time.deltaTime);

        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
