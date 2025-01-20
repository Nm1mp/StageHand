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
        // Singleton pattern to ensure only one instance persists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Mark this GameObject as persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
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
                if (key == "K" || key == "S") // Potentiometer values
                {
                    if (int.TryParse(keyValue[1], out int potValue))
                    {
                        potentiometerValues[key] = potValue;
                    }
                }
                else if (int.TryParse(key, out int button)) // Button values
                {
                    if (int.TryParse(keyValue[1], out int buttonState))
                    {
                        buttonStates[button] = buttonState;
                    }
                }
            }
        }
    }

    public int GetPotentiometerValue(string key)
    {
        if (potentiometerValues.ContainsKey(key))
        {
            return potentiometerValues[key];
        }
        Debug.LogWarning($"Potentiometer type {key} not found.");
        return 0;
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
