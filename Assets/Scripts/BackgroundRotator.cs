using UnityEngine;

public class BackgroundRotator : MonoBehaviour
{
    public Transform backgroundParent; 
    public float rotationSpeed = 90f;  
    private bool isRotating = false;   

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isRotating)
        {
            StartCoroutine(RotateBackground(90f)); 
        }
    }

    private System.Collections.IEnumerator RotateBackground(float angle)
    {
        isRotating = true; 

        float targetAngle = backgroundParent.eulerAngles.y + angle;
        while (Mathf.Abs(Mathf.DeltaAngle(backgroundParent.eulerAngles.y, targetAngle)) > 0.1f)
        {
            float step = rotationSpeed * Time.deltaTime; 
            backgroundParent.rotation = Quaternion.RotateTowards(
                backgroundParent.rotation,
                Quaternion.Euler(0, targetAngle, 0),
                step
            );
            yield return null; 
        }

        
        backgroundParent.rotation = Quaternion.Euler(0, targetAngle, 0);

        isRotating = false; 
    }
}


