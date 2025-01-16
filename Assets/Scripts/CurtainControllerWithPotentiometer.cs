using UnityEngine;
using System.IO.Ports;

using UnityEngine.SceneManagement;

public class CurtainControllerWithPotentiometer : MonoBehaviour
{
    public Transform leftCurtain;
    public Transform rightCurtain;
    public float moveSpeed = 2f;

    private SerialPort serialPort;
    [SerializeField] private string portName = "COM5";
    [SerializeField] private int baudRate = 9600;

    private Scene1Test scene1Test; // Cached reference to the Scene1Test script
    private bool isLogLifted = false;

    private float leftClosedPositionX = 0f;
    private float leftOpenPositionX = -19f;
    private float rightClosedPositionX = 10f;
    private float rightOpenPositionX = 32f;

    private float lastPotValue = -1;
    private float timeCurtainsClosed = 0f;

    void Start()
    {
        scene1Test = FindObjectOfType<Scene1Test>();

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
        if (scene1Test != null)
        {
            isLogLifted = scene1Test.IsLogLifted();
        }

        if (isLogLifted && serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine().Trim();
                if (int.TryParse(data, out int potValue) && potValue != lastPotValue)
                {
                    lastPotValue = potValue;

                    float normalizedValue = Mathf.Clamp01(potValue / 1023f);

                    float leftCurtainX = Mathf.Lerp(leftClosedPositionX, leftOpenPositionX, 1 - normalizedValue);
                    float rightCurtainX = Mathf.Lerp(rightClosedPositionX, rightOpenPositionX, 1 - normalizedValue);

                    leftCurtain.localPosition = new Vector3(leftCurtainX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
                    rightCurtain.localPosition = new Vector3(rightCurtainX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);

                    if (leftCurtainX == leftClosedPositionX && rightCurtainX == rightClosedPositionX)
                    {
                        timeCurtainsClosed += Time.deltaTime;
                        if (timeCurtainsClosed >= 2f)
                        {
                            SceneManager.LoadScene("Scene2");
                        }
                    }
                    else
                    {
                        timeCurtainsClosed = 0f;
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
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



