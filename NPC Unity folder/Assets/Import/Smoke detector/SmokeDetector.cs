using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDetector : MonoBehaviour
{
    [SerializeField] GameObject lightObject, sprinklerParticles1, sprinklerParticles2;
    Material lightMat;

    bool lightOn = false;
    public bool fireOn = false;

    void Start()
    {
        lightMat = lightObject.GetComponent<MeshRenderer>().material;
    }

    void Update()
    {
        if(fireOn == true)
        {
            sprinklerParticles1.SetActive(true);
            sprinklerParticles2.SetActive(true);
        }
        else
        {
            sprinklerParticles1.SetActive(false);
            sprinklerParticles2.SetActive(false);
        }

        //Blinking light
        if(lightOn == true)
        {
            StartCoroutine(turnOff());
        }else if(lightOn == false)
        {
            StartCoroutine(turnOn());
        }
    }

    IEnumerator turnOn()
    {
        lightMat.EnableKeyword("_EMISSION");
        //Debug.Log("Turn ON");
        yield return new WaitForSeconds(1);
        lightOn = true;
    }

    IEnumerator turnOff()
    {
        lightMat.DisableKeyword("_EMISSION");
        //Debug.Log("Turn OFF");
        yield return new WaitForSeconds(10);
        lightOn = false;
    }
}
