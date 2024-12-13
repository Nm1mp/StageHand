using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRotationTestScript : MonoBehaviour
{
    [SerializeField] GameObject prop1;
    private bool isProp = false;
    public float propSpeed = 50f;

    void Update()
    {
        if (Input.GetKey(KeyCode.X))
        {
            isProp = !isProp;
        }


        if (isProp)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * propSpeed);
            //if (prop1 = Vector3)
            //{
            //
            //}
        }
    }
}
