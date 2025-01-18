using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Test : MonoBehaviour
{
    public Transform TreeTrunk;
    [HideInInspector] public bool onGround = false;
    public float moveSpeed = 2f;
    private Vector3 trunkDisappear = new Vector3(1, 13, 6);

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
