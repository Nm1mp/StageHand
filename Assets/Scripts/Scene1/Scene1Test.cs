using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scene1Test : MonoBehaviour
{
    public Transform TreeTrunk;
    public float moveSpeed = 2f;

   [HideInInspector] public bool onGround = false; 
    private bool isLifting = false; 

    private Vector3 trunkDisappear = new Vector3(1, 13, 6.66f);

    void Update()
    {
        if (!onGround && Input.GetKeyDown(KeyCode.X))
        {
            isLifting = true;
        }

        if (isLifting)
        {
            MoveLog();
        }
    }

    private void MoveLog()
    {
        TreeTrunk.localPosition = Vector3.MoveTowards(TreeTrunk.localPosition, trunkDisappear, moveSpeed * Time.deltaTime);

        if (TreeTrunk.localPosition == trunkDisappear)
        {
            onGround = true; 
            isLifting = false; 
        }
    }

    public bool IsLogLifted()
    {
        return onGround;
    }
}

