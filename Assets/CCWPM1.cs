using System.IO.Ports;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CCWPM1 : MonoBehaviour
{
    public Transform leftCurtain;
    public Transform rightCurtain;
    public float moveSpeed = 2f;
    public Scene1Test scene1Test;

    private Vector3 leftClosedPosition = new Vector3(0, 0, 0);
    private Vector3 leftOpenPosition = new Vector3(-11, 0, 0);
    private Vector3 rightClosedPosition = new Vector3(10, 0, 0);
    private Vector3 rightOpenPosition = new Vector3(22, 0, 0);

    private SerialPort serialPort;
    [SerializeField] private string portName = "COM5";
    [SerializeField] private int baudRate = 9600;

    private int potentiometerValue = 0;
    private bool isLogLifted = false;
    private float curtainsClosedTime = 0f;
    private const float transitionDelay = 2f;

    private bool curtainsAtClosedPosition = false;

    void Start()
    {
        if (scene1Test != null)
        {
            scene1Test = FindObjectOfType<Scene1Test>();
        }

        leftCurtain.localPosition = leftOpenPosition;
        rightCurtain.localPosition = rightOpenPosition;

        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error opening serial port: " + e.Message);
        }
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
                if (int.TryParse(data, out int potValue))
                {
                    potentiometerValue = potValue;

                    UpdateCurtainsBasedOnPotentiometer();
                    CheckCurtainClosedForSceneTransition("Scene2");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
    }

    private void UpdateCurtainsBasedOnPotentiometer()
    {
        float normalizedValue = Mathf.Clamp01(potentiometerValue / 1023f);

        float leftCurtainX = Mathf.Lerp(leftClosedPosition.x, leftOpenPosition.x, 1 - normalizedValue);
        float rightCurtainX = Mathf.Lerp(rightClosedPosition.x, rightOpenPosition.x, 1 - normalizedValue);

        leftCurtain.localPosition = new Vector3(leftCurtainX, leftCurtain.localPosition.y, leftCurtain.localPosition.z);
        rightCurtain.localPosition = new Vector3(rightCurtainX, rightCurtain.localPosition.y, rightCurtain.localPosition.z);

        curtainsAtClosedPosition = Mathf.Approximately(leftCurtain.localPosition.x, leftClosedPosition.x) &&
                                    Mathf.Approximately(rightCurtain.localPosition.x, rightClosedPosition.x);

        if (curtainsAtClosedPosition)
        {
            curtainsClosedTime += Time.deltaTime;
        }
        else
        {
            curtainsClosedTime = 0f;
        }
    }

    private void CheckCurtainClosedForSceneTransition(string nextScene)
    {
        if (curtainsClosedTime >= transitionDelay && curtainsAtClosedPosition)
        {
            Debug.Log("Curtains closed. Transitioning to " + nextScene);
            SceneManager.LoadScene(nextScene);
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
