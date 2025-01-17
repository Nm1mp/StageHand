using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMovement : MonoBehaviour
{
    public Transform cat;
    public float moveSpeed = 2f;

    private bool isMoving = false;
    private bool isReturning = false;

    private Vector3 Cat = new Vector3 (10, -3, 4.5f);
  

    void Update()
    {
        if (!isMoving && Input.GetKeyDown(KeyCode.Z))
        {
            isReturning = true;
        }
        if (isReturning)
        {
            MoveCat();
        }

    }
    private void MoveCat()
    {
        cat.localPosition = Vector3.MoveTowards(cat.localPosition, Cat, moveSpeed * Time.deltaTime);

        if (cat.localPosition == Cat)
        {
            isMoving = true;
            isReturning = false;
        }
    }
}
