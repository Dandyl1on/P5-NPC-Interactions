using System.Collections;
using System.Collections.Generic;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PCMouseScript : MonoBehaviour
{
    public GameObject mouseObject;
    public float sensitivity;

    public float maxY, maxX;

    [SerializeField]GameObject vrControllerHandRight;
    ControllerAnimator yoinkScript;

    private void Start()
    {
        yoinkScript = vrControllerHandRight.GetComponent<ControllerAnimator>();
    }

    void Update()
    {
        Vector3 followPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        followPosition.x = Mathf.Clamp(transform.localPosition.x, transform.localPosition.x - maxX, transform.localPosition.x + maxX);
        followPosition.y = Mathf.Clamp(transform.localPosition.y, transform.localPosition.y - maxY, transform.localPosition.y + maxY);

        transform.localPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        //Debug.Log(yoinkScript.m_TriggerInput);
    }

    //InputDevice

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("popup"))
        {

            Debug.Log("Entering!");
        }

        if (other.gameObject.CompareTag("popupVirus"))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("popup"))
        {
            Debug.Log("Exiting!");
        }
    }
}
