using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    private SerialPort serialPort;
    [SerializeField] private string portName = "COM4";
    [SerializeField] private int baudRate = 9600;

    private Dictionary<int, int> buttonStates = new Dictionary<int, int>();
    private Dictionary<string, int> potentiometerValues = new Dictionary<string, int>();
    public static SerialPortManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure the manager persists across scenes
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
                string data = serialPort.ReadLine().Trim(); // Example: "2:1;4:0;3:1;P:512;S:300;"
                ParseData(data);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
    }

    private void ParseData(string data)
    {
        string[] keyValuePairs = data.Split(';');
        foreach (string pair in keyValuePairs)
        {
            if (string.IsNullOrEmpty(pair)) continue;

            string[] keyValue = pair.Split(':');
            if (keyValue.Length == 2)
            {
                string key = keyValue[0];

                // Parse button states
                if (int.TryParse(key, out int button))
                {
                    if (int.TryParse(keyValue[1], out int buttonState))
                    {
                        buttonStates[button] = buttonState;
                    }
                }
                // Parse potentiometer values
                else if (key == "K" || key == "S")
                {
                    if (int.TryParse(keyValue[1], out int potValue))
                    {
                        potentiometerValues[key] = potValue;
                    }
                }
            }
        }
    }

    public bool IsButtonPressed(int buttonNumber)
    {
        return buttonStates.ContainsKey(buttonNumber) && buttonStates[buttonNumber] == 1;
    }

    public int GetPotentiometerValue(string type)
    {
        if (potentiometerValues.ContainsKey(type))
        {
            return potentiometerValues[type];
        }
        else
        {
            Debug.LogWarning($"Potentiometer type {type} not found.");
            return -1; // Default invalid value
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
