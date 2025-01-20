using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    // Plant things
    public GameObject Plant;
    //public GameObject Seed;
    private bool plantSpawned = false;
    [Space]
    // Bird things
    public Transform Bird;
    private float moveSpeed = 2.8f;
    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 6.58f);
    [Space]
    // Drum things
    public Transform Drum;
    private float smooth = 2.8f;
    private Vector3 drumMove = new Vector3(5.12f, -2.786f, 3.46f);
    // Rain things
    private bool rainStarted = false;
    [Space]
    public ParticleSystem Rain;
    public float RainFlow;
    private int rainTime = 3;
    [Space]
    // Cow things
    public Transform Cow;
    private Vector3 cowMove = new Vector3(6.14f, -1.02f, 7.36f);
    [Space]
    // Trumpet things
    public Transform Trumpet;
    private Vector3 trumpMove = new Vector3(5.032f, -3.051f, 5.184f);
    [Space]
    // Dog things
    public GameObject DogLeft;
    public GameObject DogR;
    public Transform DogRight;
    private Vector3 dogMove = new Vector3(5.03200006f, -2.56f, 5.18400002f);
    [Space]
    public GameObject DonkSad_L;
    public GameObject DonkExcited_L;
    [Space]
    public GameObject DonkSad_R;
    public GameObject DonkExcited_R;



    void Update()
    {
        bool button5Pressed = SerialPortManager.Instance.IsButtonPressed(5);

        if (Plant != null) //&& Seed != null)
        {
            if (Input.GetKeyDown(KeyCode.C) || button5Pressed)
            {
                if (!rainStarted)
                {
                    //Debug.Log("Rain started");

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
                //Debug.Log("Rain stop");

                Rain.Stop();
                rainStarted = false;

                Plant.SetActive(true);
                //Seed.SetActive(false);
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
                        //Debug.Log("bird landed");
                        //Debug.Log("cow landed");
                        plantSpawned = true;
                    }
                    //Debug.Log("Bush is active");
                }
                else if (Cow != null && Trumpet != null) 
                {
                    Cow.localPosition = Vector3.MoveTowards(Cow.localPosition, cowMove, moveSpeed * Time.deltaTime);
                    Trumpet.localPosition = Vector3.MoveTowards(Trumpet.localPosition, trumpMove, moveSpeed * Time.deltaTime);
                    Quaternion goal = Quaternion.Euler(0, -180f, -83.28f);
                    Trumpet.rotation = Quaternion.Slerp(Trumpet.rotation, goal, Time.deltaTime * smooth);

                    if (Cow.localPosition == cowMove)
                    {
                        plantSpawned = true;
                    }
                }

                //Debug.Log("Flower is active");
            }

         
        }
    }
}
