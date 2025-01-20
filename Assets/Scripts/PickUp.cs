using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    //scene 2
    public GameObject drum;
    public GameObject playerDrum1;
    public GameObject playerDrum2;
    [Space]
    public GameObject invisWall;
    private bool drumNotHere = false;
    [Space]
    //scene 3
    public GameObject Trumpet;
    public GameObject PlayerTrump1;
    //public GameObject PlayerTrump2;
    private bool trumpNotHere;
    [Space]
    public GameObject DonkRight;
    public GameObject DonkLeft;
    [Space]
    public GameObject DonkExicite_L;
    public GameObject DonkExicite_R;
    [Space]
    public GameObject DonkSad_L;
    public GameObject DonkSad_R;


    private void OnTriggerEnter(Collider other)
    {
        if (drum != null)
        {
            if (drum.activeSelf && !drumNotHere)
            {
                if (DonkRight.activeSelf)
                {
                    playerDrum2.SetActive(true);
                    DonkSad_R.SetActive(false);
                    DonkExicite_R.SetActive(true);
                }

                if (DonkLeft.activeSelf)
                {
                    playerDrum1.SetActive(true);
                    DonkSad_L.SetActive(false);
                    DonkExicite_L.SetActive(true);
                }

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
                PlayerTrump1.SetActive(true);
                //PlayerTrump2.SetActive(true);
                Trumpet.SetActive(false);
                invisWall.SetActive(false);
            }
            else
            {
                trumpNotHere = true;
            }
        }
    }
}
