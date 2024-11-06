using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PCMouseScript : MonoBehaviour
{
    public GameObject mouseObject;
    public float sensitivity;

    public float maxY, maxX;
    
    void Update()
    {
        Vector3 poo = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        poo.x = Mathf.Clamp(transform.localPosition.x, transform.localPosition.x - maxX, transform.localPosition.x + maxX);
        poo.y = Mathf.Clamp(transform.localPosition.y, transform.localPosition.y - maxY, transform.localPosition.y + maxY);

        transform.localPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);
    }

    //InputDevice

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
