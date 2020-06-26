using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PControlTest : MonoBehaviour
{
    public float _speed = 6.5f;
    public Rigidbody rb;
    Vector3 movement;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");       
        movement.z = Input.GetAxisRaw("Vertical"); 
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * _speed * Time.fixedDeltaTime);
    }
}
