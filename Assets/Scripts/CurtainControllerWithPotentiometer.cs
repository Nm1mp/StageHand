using UnityEngine;
using UnityEngine.SceneManagement;

public class CurtainControllerWithPotentiometer : MonoBehaviour
{
    public Transform leftCurtain;
    public Transform rightCurtain;
    public float moveSpeed = 2f;

    [Header("Scene 1 - Log Settings")]
    public bool isScene1 = false;
    public Scene1Test scene1Test;

    [Header("Scene 2 - Drum Settings")]
    public bool isScene2 = false;
    public GameObject drum;
    public GameObject playerDrum;

    [Header("Scene 3 - Trumpet Settings")]
    public bool isScene3 = false;
    public GameObject trumpet;
    public GameObject playerTrumpet;

    [Header("Scene 4 - Violin Settings")]
    public bool isScene4 = false;
    public GameObject violin;
    public GameObject playerViolin;

    [Header("Curtain Positions")]
    private float leftClosedPositionX = 0f;
    private float leftOpenPositionX = -11f;
    private float rightClosedPositionX = 10f;
    private float rightOpenPositionX = 22f;

    private bool curtainsOpening = true;

    void Start()
    {
        leftCurtain.localPosition = new Vector3(leftClosedPositionX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightClosedPositionX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);

        curtainsOpening = true;

        if (isScene1 && scene1Test != null)
        {
            scene1Test.OnLogLifted += HandleLogLifted;
        }
    }

    void Update()
    {
        if (curtainsOpening)
        {
            OpenCurtains();
            return;
        }

        if (SerialPortManager.Instance != null)
        {
            int knobValue = SerialPortManager.Instance.GetPotentiometerValue("K");

            if (isScene1 && CheckLogLifted())
            {
                MoveCurtains(knobValue);
                CheckCurtainClosedForScene2();
            }
            else if (isScene2 && playerDrum != null && playerDrum.activeSelf)
            {
                MoveCurtains(knobValue);
                CheckCurtainClosedForScene3();
            }
            else if (isScene3 && playerTrumpet != null && playerTrumpet.activeSelf)
            {
                MoveCurtains(knobValue);
                CheckCurtainClosedForScene4();
            }
            else if (isScene4 && playerViolin != null && playerViolin.activeSelf)
            {
                MoveCurtains(knobValue);
                CheckCurtainClosedForScene5();
            }
        }
        else
        {
            Debug.LogWarning("SerialPortManager instance is null.");
        }
    }

    private void OpenCurtains()
    {
        leftCurtain.localPosition = Vector3.MoveTowards(leftCurtain.localPosition,
            new Vector3(leftOpenPositionX, leftCurtain.localPosition.y, leftCurtain.localPosition.z), moveSpeed * Time.deltaTime);
        rightCurtain.localPosition = Vector3.MoveTowards(rightCurtain.localPosition,
            new Vector3(rightOpenPositionX, rightCurtain.localPosition.y, rightCurtain.localPosition.z), moveSpeed * Time.deltaTime);

        if (Mathf.Approximately(leftCurtain.localPosition.x, leftOpenPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightOpenPositionX))
        {
            curtainsOpening = false; 
        }
    }

    private bool CheckLogLifted()
    {
        return scene1Test != null && scene1Test.IsLogLifted();
    }

    private void HandleLogLifted()
    {
        Debug.Log("Log lifted event received.");
        curtainsOpening = false;
    }

    private void MoveCurtains(int potValue)
    {
        float normalizedValue = Mathf.Clamp01(potValue / 1023f);

        float leftCurtainX = Mathf.Lerp(leftClosedPositionX, leftOpenPositionX, 1 - normalizedValue);
        float rightCurtainX = Mathf.Lerp(rightClosedPositionX, rightOpenPositionX, 1 - normalizedValue);

        leftCurtain.localPosition = new Vector3(leftCurtainX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightCurtainX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);
    }

    private void CheckCurtainClosedForScene2()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 2.");
            SceneManager.LoadScene("Scene2");
        }
    }

    private void CheckCurtainClosedForScene3()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 3.");
            SceneManager.LoadScene("Scene3");
        }
    }

    private void CheckCurtainClosedForScene4()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 4.");
            SceneManager.LoadScene("Scene4");
        }
    }
    private void CheckCurtainClosedForScene5()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
                       Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 5.");
            SceneManager.LoadScene("Scene5");
        }
    }
}
