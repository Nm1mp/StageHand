using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Scene1Test : MonoBehaviour
{
    public Transform TreeTrunk;
    public float moveSpeed = 2f;

    private bool onGround = false; 
    private bool isLifting = false; 

    private Vector3 trunkDisappear = new Vector3(2, 13, 4.8f);

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

