using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //scene 2
    public GameObject drum;
    public GameObject playerDrum;
    public GameObject invisWall;
    private bool drumNotHere = false;
    //scene 3
    public GameObject Trumpet;
    public GameObject PlayerTrump;
    private bool trumpNotHere;



    private void OnTriggerEnter(Collider other)
    {
        if (drum != null)
        {
            if (drum.activeSelf && !drumNotHere)
            {
                playerDrum.SetActive(true);
                drum.SetActive(false);
                invisWall.SetActive(false);
                
            }
            else
            {
                drumNotHere = true;
            }
        }

        if (Trumpet != null)
        {
            if (Trumpet.activeSelf && !trumpNotHere)
            {
                PlayerTrump.SetActive(true);
                Trumpet.SetActive(false);
                //invisWall.SetActive(false);
            }
            else
            {
                trumpNotHere = true;
            }
        }
    }
}
