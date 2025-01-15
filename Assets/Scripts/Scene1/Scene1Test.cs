using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Test : MonoBehaviour
{
    public Transform TreeTrunk;
    private bool onGround = false;
    public float moveSpeed = 2f;
    private Vector3 trunkDisappear = new Vector3(2, 13, 4.8f);

   void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            onGround = true;
        }

        if(onGround == true)
        {
            TreeTrunk.localPosition = Vector3.MoveTowards(TreeTrunk.localPosition, trunkDisappear, moveSpeed * Time.deltaTime);
        }
    }
}
