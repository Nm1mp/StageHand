using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(horizontalInput, 0, 0) * speed * Time.deltaTime;
        transform.Translate(movement);

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }
}