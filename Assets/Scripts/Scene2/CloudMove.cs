using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CloudMove : MonoBehaviour
{
    // Plant things
    public GameObject Plant;
    public GameObject Seed;
    private bool plantSpawned = false;
    [Space]
    // Bird things
    public Transform Bird;
    private float moveSpeed = 3.8f;
    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 6.58f);
    [Space]
    // Drum things
    public Transform Drum;
    private float smooth = 3.8f;
    private Vector3 drumMove = new Vector3(5.12f, -2.786f, 3.46f);
    [Space]
    // Rain things
    private bool rainStarted = false;
    public ParticleSystem Rain;
    public float RainFlow;
    private int rainTime = 3;
    [Space]
    // Cow things
    public Transform Cow;
    private Vector3 cowMove = new Vector3(8.67f, -1.907f, 7.67f);
    // Trumpet things
    public Transform Trumpet;
    private Vector3 trumpMove = new Vector3(5.76999998f, -3.04900002f, 3.81999993f);
    // Dog Things
    public Transform Dog;
    private Vector3 dogMove = new Vector3(6.09f, -2.56f, 4.18f);

    void Update()
    {
        bool button5Pressed = SerialPortManager.Instance.IsButtonPressed(5);

        if (Plant != null && Seed != null)
        {
            if (Input.GetKeyDown(KeyCode.C) || button5Pressed)
            {
                if (!rainStarted)
                {
                    Rain.Play();
                    rainStarted = true;
                }
            }

            if (rainStarted)
            {
                RainFlow += (1 * Time.deltaTime);
            }

            if (RainFlow >= rainTime && rainStarted)
            {
                Rain.Stop();
                rainStarted = false;

                Plant.SetActive(true);
                Seed.SetActive(false);
            }

            if (Plant.activeSelf && !plantSpawned)
            {
                if (Bird != null && Drum != null)
                {
                    Bird.localPosition = Vector3.MoveTowards(Bird.localPosition, birdMove, moveSpeed * Time.deltaTime);
                    Drum.localPosition = Vector3.MoveTowards(Drum.localPosition, drumMove, moveSpeed * Time.deltaTime);
                    Quaternion target = Quaternion.Euler(0, 0, 0);
                    Drum.rotation = Quaternion.Slerp(Drum.rotation, target, Time.deltaTime * smooth);

                    if (Bird.localPosition == birdMove)
                    {
                        plantSpawned = true;
                    }
                }
                else if (Cow != null && Trumpet != null)
                {
                    Cow.localPosition = Vector3.MoveTowards(Cow.localPosition, cowMove, moveSpeed * Time.deltaTime);
                    Trumpet.localPosition = Vector3.MoveTowards(Trumpet.localPosition, trumpMove, moveSpeed * Time.deltaTime);
                    Quaternion goal = Quaternion.Euler(0, 0, 196.973f);
                    Trumpet.rotation = Quaternion.Slerp(Trumpet.rotation, goal, Time.deltaTime * smooth);

                    if (Cow.localPosition == cowMove)
                    {
                        plantSpawned = true;
                    }
                }
            }

            if (Trumpet != null)
            {
                if (Trumpet.localPosition == trumpMove)
                {
                    Dog.localPosition = Vector3.MoveTowards(Dog.localPosition, dogMove, moveSpeed * Time.deltaTime);
                }
            }
        }
    }
}

