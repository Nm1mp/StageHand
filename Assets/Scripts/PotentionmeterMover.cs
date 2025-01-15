using System.Collections;
using System.Collections.Generic;
using UnityEngine;



using System.IO.Ports;

public class PotentiometerMover : MonoBehaviour
{
    [SerializeField] private string portName = "COM4";
    [SerializeField] private int baudRate = 9600;
    [SerializeField] private float speed = 5f;
    [SerializeField] private Transform targetObject;

    private SerialPort serialPort;

    void Start()
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

    void Update()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                string data = serialPort.ReadLine();
                int potValue = int.Parse(data);

                if (potValue > 511)
                {
                    MoveObject(Vector3.left);
                }
                else
                {
                    MoveObject(Vector3.right);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Error reading from serial port: " + e.Message);
            }
        }
    }

    private void MoveObject(Vector3 direction)
    {
        if (targetObject != null)
        {
            targetObject.Translate(direction * speed * Time.deltaTime);
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}

