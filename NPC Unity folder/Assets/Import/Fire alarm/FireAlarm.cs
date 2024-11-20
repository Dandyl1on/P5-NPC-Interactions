using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarm : MonoBehaviour
{
    [SerializeField] GameObject sprinklerParticles1, sprinklerParticles2;
    [SerializeField] AudioSource fireAlarmSound;
    public bool fireOn;
    bool alarmIsPlaying;

    void Start()
    {
        fireOn = false;
        alarmIsPlaying = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireOn == true)
        {
            sprinklerParticles1.SetActive(true);
            sprinklerParticles2.SetActive(true);

            if(alarmIsPlaying == false)
            {
                fireAlarmSound.Play();
                alarmIsPlaying = true;
            }
        }
        else
        {
            sprinklerParticles1.SetActive(false);
            sprinklerParticles2.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hand"))
        {
            fireOn = true;
        }    
    }
}
