using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioPickUp : MonoBehaviour
{
    public AudioSource Piano;
    public AudioSource Percussion;
    public AudioSource Brass;
    public AudioSource Strings;

    public bool PianoPlaying = true;
    public bool PercussionPlaying = false;
    public bool BrassPlaying = false;
    public bool StringsPlaying = false;

    public float minVolume = 0.0f;
    public float MaxVolume = 0.46f;

    public GameObject playerDrum1;
    public GameObject playerDrum2;
    public GameObject playerTrumpet1;
    public GameObject playerTrumpet2;
    public GameObject playerViolin;

    bool DrumPickUp = false;
    bool TrumpetPickUp = false;
    bool ViolinPickUp = false;


    void Awake()
    {
        Percussion.volume = 0.0f;
        Brass.volume = 0.0f;
        Strings.volume = 0.0f;

        //Debug.Log(Percussion.volume);
        //Debug.Log(Strings.volume); 
        //Debug.Log(Brass.volume);
    }

    private void Update()
    {
        
        if (playerDrum1.activeSelf && DrumPickUp == false || playerDrum2.activeSelf && DrumPickUp == false)
        {
            PercussionPlaying = true;
            PercussionToggle();
            DrumPickUp = true;
        }

        if (playerTrumpet1 != null && playerTrumpet1.activeSelf && TrumpetPickUp == false || playerTrumpet2 != null && playerTrumpet2.activeSelf && TrumpetPickUp == false)
        {
            BrassPlaying = true;
            BrassToggle();
            TrumpetPickUp = true;
        }

        if (playerViolin != null && playerViolin.activeSelf && ViolinPickUp == false)
        {
            StringsPlaying = true;
            StringsToggle();
            ViolinPickUp = true;
        }

        PianoToggle();

        if (Input.GetKeyDown(KeyCode.O))
        {
            PercussionPlaying = !PercussionPlaying;
            PercussionToggle();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            BrassPlaying = !BrassPlaying;
            BrassToggle();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            StringsPlaying = !StringsPlaying;
            StringsToggle();
        }

    }

    public void PianoToggle()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PianoPlaying = !PianoPlaying;
            if(PianoPlaying == true)
            {
                Piano.volume = MaxVolume;
            }
            else
            {
                Piano.volume = minVolume;
            }
        }


    }

    public void PercussionToggle()
    {
        if (PercussionPlaying == true)
        {
            Percussion.volume = MaxVolume;
        }
        else
        {
            Percussion.volume = minVolume;
        }
    }

    public void BrassToggle()
    {
        if (BrassPlaying == true)
        {
            Brass.volume = MaxVolume;
        }
        else
        {
            Brass.volume = minVolume;
        }
    }

    public void StringsToggle()
    {
        if (StringsPlaying == true)
        {
            Strings.volume = MaxVolume;
        }
        else
        {
            Strings.volume = minVolume;
        }
    }

}
