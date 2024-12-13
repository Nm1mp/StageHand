using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objMover : MonoBehaviour
{
    public GameObject test;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        Vector3 velocity = new Vector3 (moveX * speed, rb.velocity.y, rb.velocity.z);
        rb.velocity = velocity;
        
    }
    
}
