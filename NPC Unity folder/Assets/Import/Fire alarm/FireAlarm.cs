using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAlarm : MonoBehaviour
{
    [SerializeField] GameObject sprinklerParticles1, sprinklerParticles2;
    [SerializeField] AudioSource fireAlarmSound;
    AudioSource sprinkler1Sound, sprinkler2Sound;
    public bool fireOn;
    bool alarmIsPlaying;

    public GameObject Fire;

    void Start()
    {
        fireOn = false;
        alarmIsPlaying = false;
        sprinkler1Sound = sprinklerParticles1.GetComponent<AudioSource>();
        sprinkler2Sound = sprinklerParticles2.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireOn == true)
        {
            sprinklerParticles1.SetActive(true);
            sprinklerParticles2.SetActive(true);

            if (alarmIsPlaying == false)
            {
                fireAlarmSound.Play();
                sprinkler1Sound.Play();
                sprinkler2Sound.Play();
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
            if (Fire.active == false)
            {
                FindObjectOfType<NPCBrain>().Temperment += 100;
            }
            fireOn = true;
        }    
    }
}
