using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeDetector : MonoBehaviour
{
    [SerializeField] GameObject light;
    Material lightMat;

    bool lightOn = false;

    void Start()
    {
        lightMat = light.GetComponent<Material>();
    }

    void Update()
    {
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
        yield return new WaitForSeconds(1);
        lightOn = false;
    }

    IEnumerator turnOff()
    {
        lightMat.DisableKeyword("_EMISSION");
        yield return new WaitForSeconds(1);
        lightOn = true;
    }
}
