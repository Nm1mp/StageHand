using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class SerialPortManager : MonoBehaviour
{
    private SerialPort serialPort;
    [SerializeField] private string portName = "COM4"; // Adjust your port
    [SerializeField] private int baudRate = 9600;

    private Dictionary<int, int> buttonStates = new Dictionary<int, int>();
    private int potentiometerValue = 0;

    public static SerialPortManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make persistent across scenes
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
                string data = serialPort.ReadLine().Trim(); // Example format: "2:1;4:0;P:512;"
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
        string[] entries = data.Split(';');
        foreach (string entry in entries)
        {
            if (string.IsNullOrEmpty(entry)) continue;

            if (entry.StartsWith("P:")) // Potentiometer data
            {
                string potValueStr = entry.Substring(2);
                if (int.TryParse(potValueStr, out int potValue))
                {
                    potentiometerValue = potValue;
                }
            }
            else // Button data
            {
                string[] parts = entry.Split(':');
                if (parts.Length == 2 && int.TryParse(parts[0], out int button) && int.TryParse(parts[1], out int state))
                {
                    buttonStates[button] = state;
                }
            }
        }
    }

    public bool IsButtonPressed(int buttonNumber)
    {
        return buttonStates.ContainsKey(buttonNumber) && buttonStates[buttonNumber] == 1;
    }

    public int GetPotentiometerValue()
    {
        return potentiometerValue;
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }
}
