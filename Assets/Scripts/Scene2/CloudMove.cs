using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMove : MonoBehaviour
{
    // Objects
    public GameObject Plant;
    public GameObject Seed;
    private bool plantSpawned = false;
    [Space]
    // Bird things
    public Transform Bird;
    private float moveSpeed = 3.8f;
    private Vector3 birdMove = new Vector3(-0.13f, -2.763f, 4.33f);
    [Space]
    // Drum things
    public Transform Drum;
    private float smooth = 3.8f;
    private Vector3 drumMove = new Vector3(5.12f, -2.786f, 2.38f);
    [Space]
    // Rain things
    private bool rainStarted = false;
    public ParticleSystem Rain;
    public float RainFlow;
    private int rainTime = 3;
    [Space]
    // Cow things
    public Transform Cow;
    private Vector3 cowMove = new Vector3(8.67f, -1.907f, 7.15f);
    // Trumpet things
    public Transform Trumpet;




    void Update()
    {
        if (Plant != null && Seed != null) {
            if (Input.GetKeyDown(KeyCode.C))
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
                        //Debug.Log("bird landed");
                        //Debug.Log("cow landed");
                        plantSpawned = true;
                    }
                    //Debug.Log("Bush is active");
                }
                else if (Cow != null) 
                {
                    Cow.localPosition = Vector3.MoveTowards(Cow.localPosition, cowMove, moveSpeed * Time.deltaTime);
                    
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
