using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour {

    public float speed;
    public float rotationSpeed;


    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        transform.Rotate(Vector3.up * inputHorizontal * rotationSpeed);
        transform.position = transform.position + transform.forward * speed * inputVertical;
    }
}
