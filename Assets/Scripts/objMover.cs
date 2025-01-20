using UnityEngine;

public class objMover : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float minZ = -8f; 
    [SerializeField] private float maxZ = 8f;  

    void Update()
    {
        float verticalInput = Input.GetAxis("Horizontal");

        if (SerialPortManager.Instance.IsButtonPressed(4)) verticalInput = 1;  
        if (SerialPortManager.Instance.IsButtonPressed(2)) verticalInput = -1; 

        Vector3 movement = new Vector3(0, 0, verticalInput) * speed * Time.deltaTime;
        transform.Translate(movement);

        float clampedZ = Mathf.Clamp(transform.position.z, minZ, maxZ);
        transform.position = new Vector3(transform.position.x, transform.position.y, clampedZ);
    }
}

