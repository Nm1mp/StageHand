/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself. In case you are fond of that.
 */
public class SampleUserPolling_JustRead : MonoBehaviour
{
    public SerialController serialController;
    public GameObject Cube1;
    public GameObject Cube2;
    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Executed each frame
    void Update()
    {
        string message = serialController.ReadSerialMessage();

        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
        {
            //Debug.Log("Message arrived: " + message);
            var c1x = Map(float.Parse(message), 0, 1023, 4.3f, 13);
            var c2x = Map(float.Parse(message), 0, 1023, -4.3f, -13);
            Cube1.transform.position = new Vector3(c1x, 0, 0);
            Cube2.transform.position = new Vector3(c2x, 0, 0);
        }
    }

    float Map(float s, float old1, float old2, float new1, float new2)
    {
        return new1 + (s - old1) * (new2 - new1) / (old2 - old1);
    }
}
