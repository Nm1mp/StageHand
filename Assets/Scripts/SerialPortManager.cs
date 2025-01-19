using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    private SerialPort serialPort;
    [SerializeField] private string portName = "COM4";
    [SerializeField] private int baudRate = 9600;

    private Dictionary<int, int> buttonStates = new Dictionary<int, int>();

    public static SerialPortManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
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
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine().Trim(); 
                ParseButtonStates(data);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
    }
    public int GetPotentiometerValue()
    {
        if (buttonStates.ContainsKey(0)) 
        {
            return buttonStates[0];
        }
        return 0;
    }

    private void ParseButtonStates(string data)
    {
        string[] buttonDataArray = data.Split(';');
        foreach (string buttonData in buttonDataArray)
        {
            if (string.IsNullOrEmpty(buttonData)) continue;

            string[] keyValue = buttonData.Split(':');
            if (keyValue.Length == 2 && int.TryParse(keyValue[0], out int button) && int.TryParse(keyValue[1], out int state))
            {
                buttonStates[button] = state;
            }
        }
    }

    public bool IsButtonPressed(int buttonNumber)
    {
        return buttonStates.ContainsKey(buttonNumber) && buttonStates[buttonNumber] == 1;
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
