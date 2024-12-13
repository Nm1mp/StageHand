using UnityEngine;

public class CurtainController : MonoBehaviour
{
    public Transform leftCurtain;
    public Transform rightCurtain;
    public float moveSpeed = 2f;
    private bool isOpen = false; 

    
    private Vector3 leftClosedPosition = new Vector3(-3, 2, 0);
    private Vector3 rightClosedPosition = new Vector3(3, 2, 0);
    private Vector3 leftOpenPosition = new Vector3(-7, 10, 0);
    private Vector3 rightOpenPosition = new Vector3(7, 10, 0);

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOpen = !isOpen; 
        }

        
        if (isOpen)
        {
            
            leftCurtain.localPosition = Vector3.MoveTowards(leftCurtain.localPosition, leftOpenPosition, moveSpeed * Time.deltaTime);
            rightCurtain.localPosition = Vector3.MoveTowards(rightCurtain.localPosition, rightOpenPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            //Debug.Log("working");
            
            leftCurtain.localPosition = Vector3.MoveTowards(leftCurtain.localPosition, leftClosedPosition, moveSpeed * Time.deltaTime);
            rightCurtain.localPosition = Vector3.MoveTowards(rightCurtain.localPosition, rightClosedPosition, moveSpeed * Time.deltaTime);
        }
    }
}

