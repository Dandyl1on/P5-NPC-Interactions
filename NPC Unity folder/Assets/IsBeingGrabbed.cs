using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IsBeingGrabbed : MonoBehaviour
{
    public NPCJob NPCDoGrabReation;

    Rigidbody rb;
    public bool hasAlerted;
    public float throwThreshold;

    XRGrabInteractable interactable;
    public NPCBrain NPCBrain;
    public bool Once;

    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        NPCBrain = FindObjectOfType<NPCBrain>();

        rb = GetComponent<Rigidbody>();
        throwThreshold = 7.5f;
    }

    void Update()
    {
        if (NPCBrain.SmartBrain == true)
        {
            if (Once == false)
            {
                if (interactable.Grabbed == true)
                {
                    if (NPCDoGrabReation.Name != "")
                    {
                        NPCBrain.TheNPCShouldDoAnotherThing(NPCDoGrabReation);

                        Once = true;
                    }
                }
            }

            if (interactable.Dropped == true)
            {
                if (rb.angularVelocity.magnitude > throwThreshold && hasAlerted == false)
                {
                    NPCBrain.TheNPCShouldDoAnotherThing(NPCBrain.Throw);
                    StartCoroutine(AlertCoolDown());

                    NPCBrain.Temperment += 10;
                }
            }
        }
    }

    IEnumerator AlertCoolDown()
    {
        hasAlerted = true;
        yield return new WaitForSeconds(1.5f);
        hasAlerted = false;
    }
}
