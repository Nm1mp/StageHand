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

    [Header("Scene 2 - Drum Settings")]
    public bool isScene2 = false;
    public GameObject drum;
    public GameObject playerDrum;

    [Header("Scene 3 - Trumpet Settings")]
    public bool isScene3 = false;
    public GameObject trumpet;
    public GameObject playerTrumpet;

    [Header("Curtain Positions")]
    private float leftClosedPositionX = 0f;
    private float leftOpenPositionX = -11f;
    private float rightClosedPositionX = 10f;
    private float rightOpenPositionX = 22f;

    private SerialPort serialPort;
    [SerializeField] private string portName = "COM4";
    [SerializeField] private int baudRate = 9600;

    private bool curtainsOpening = true;
    private float lastPotValue = -1;

    void Awake()
    {
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
    }

    void Start()
    {
        // Initialize curtain positions
        leftCurtain.localPosition = new Vector3(leftClosedPositionX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightClosedPositionX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);

        if (isScene1)
        {
            scene1Test = FindObjectOfType<Scene1Test>();
            if (scene1Test != null)
            {
                scene1Test.OnLogLifted += HandleLogLifted;
            }
        }
    }

    void Update()
    {
        if (curtainsOpening)
        {
            OpenCurtains();
            return;
        }

        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                if (int.TryParse(data, out int potValue))
                {
                    lastPotValue = potValue;

                    if (isScene1 && CheckLogLifted())
                    {
                        MoveCurtains(potValue);
                        CheckCurtainClosedForScene2();
                    }
                    else if (isScene2 && playerDrum.activeSelf)
                    {
                        MoveCurtains(potValue);
                        CheckCurtainClosedForScene3();
                    }
                    else if (isScene3 && playerTrumpet.activeSelf)
                    {
                        MoveCurtains(potValue);
                        CheckCurtainClosedForScene4();
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
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
            curtainsOpening = false; // Curtains have fully opened
        }
    }

    private bool CheckLogLifted()
    {
        if (scene1Test != null)
        {
            return scene1Test.IsLogLifted();
        }

        return false;
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
            CleanupSerialPort();
            SceneManager.LoadScene("Scene2");
        }
    }

    private void CheckCurtainClosedForScene3()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 3.");
            CleanupSerialPort();
            SceneManager.LoadScene("Scene3");
        }
    }

    private void CheckCurtainClosedForScene4()
    {
        if (Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPositionX) &&
            Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPositionX))
        {
            Debug.Log("Transitioning to Scene 4.");
            CleanupSerialPort();
            SceneManager.LoadScene("Scene 4");
        }
    }

    private void CleanupSerialPort()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            serialPort = null;
        }
    }

    void OnApplicationQuit()
    {
        //
        CleanupSerialPort();
    }
}
