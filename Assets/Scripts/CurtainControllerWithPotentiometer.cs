using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;

public class CurtainControllerWithPotentiometer : MonoBehaviour
{
    public Transform leftCurtain;
    public Transform rightCurtain;
    public float moveSpeed = 2f;

    [Header("Scene 1 - Log Settings")]
    public bool isScene1 = false; 
    public Scene1Test scene1Test; 
    private bool isLogLifted = false;

    [Header("Scene 2 - Drum Settings")]
    public bool isScene2 = false; 
    public GameObject drum; 
    private bool isDrumActive = false;

    [Header("Curtain Positions")]
    private float leftClosedPositionX = 0f;
    private float leftOpenPositionX = -19f;
    private float rightClosedPositionX = 10f;
    private float rightOpenPositionX = 32f;

    private SerialPort serialPort;
    [SerializeField] private string portName = "COM5";
    [SerializeField] private int baudRate = 9600;

    private float lastPotValue = -1;
    private float timeAboveThreshold = 0f;
    private const int potThreshold = 900;
    private const float holdTime = 3f;

    void Start()
    {
        if (isScene1)
        {
            scene1Test = FindObjectOfType<Scene1Test>();
        }

        serialPort = new SerialPort(portName, baudRate);
        try
        {
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }

        leftCurtain.localPosition = new Vector3(leftOpenPositionX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightOpenPositionX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);
    }

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                if (int.TryParse(data, out int potValue))
                {
                    lastPotValue = potValue;

                    if (isScene1 && CheckLogLifted(potValue)) 
                    {
                        MoveCurtains(potValue);
                    }
                    else if (isScene2 && CheckDrumActive(potValue)) 
                    {
                        MoveCurtains(potValue);
                        CheckCurtainClosedForScene3();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
    }

    private bool CheckLogLifted(int potValue)
    {
        if (scene1Test != null)
        {
            isLogLifted = scene1Test.IsLogLifted();
        }

        if (isLogLifted)
        {
            if (potValue > potThreshold)
            {
                timeAboveThreshold += Time.deltaTime;
                if (timeAboveThreshold >= holdTime)
                {
                    Debug.Log("Log lifted, curtains can close.");
                    return true;
                }
            }
            else
            {
                timeAboveThreshold = 0f;
            }
        }

        return false;
    }

    private bool CheckDrumActive(int potValue)
    {
        isDrumActive = drum != null && drum.activeSelf;

        if (isDrumActive)
        {
            if (potValue > potThreshold)
            {
                timeAboveThreshold += Time.deltaTime;
                if (timeAboveThreshold >= holdTime)
                {
                    Debug.Log("Drum active, curtains can close.");
                    return true;
                }
            }
            else
            {
                timeAboveThreshold = 0f;
            }
        }

        return false;
    }

    private void MoveCurtains(int potValue)
    {
        float normalizedValue = Mathf.Clamp01(potValue / 1023f);

        float leftCurtainX = Mathf.Lerp(leftClosedPositionX, leftOpenPositionX, 1 - normalizedValue);
        float rightCurtainX = Mathf.Lerp(rightClosedPositionX, rightOpenPositionX, 1 - normalizedValue);

        leftCurtain.localPosition = new Vector3(leftCurtainX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightCurtainX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);
    }

    private void CheckCurtainClosedForScene3()
    {
        if (leftCurtain.localPosition.x == leftClosedPositionX && rightCurtain.localPosition.x == rightClosedPositionX)
        {
            Debug.Log("Curtains fully closed, transitioning to Scene 3.");
            SceneManager.LoadScene("Scene3");
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}






