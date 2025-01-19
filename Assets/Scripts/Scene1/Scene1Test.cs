using UnityEngine;
using System;

public class Scene1Test : MonoBehaviour
{
    public Transform TreeTrunk;
    public float moveSpeed = 2f;

    private bool onGround = false;
    private bool isLifting = false;

    private Vector3 trunkDisappear = new Vector3(1, 13, 6.66f);

    public event Action OnLogLifted;

    void Update()
    {
        bool button3Pressed = SerialPortManager.Instance.IsButtonPressed(3);

        if (!onGround && (Input.GetKeyDown(KeyCode.X) || button3Pressed))
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

            OnLogLifted?.Invoke();
        }
    }

    public bool IsLogLifted()
    {
        return onGround;
    }
}
