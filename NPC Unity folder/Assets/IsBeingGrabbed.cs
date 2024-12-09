using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IsBeingGrabbed : MonoBehaviour
{
    public NPCDo NPCDoGrabReation;

    Rigidbody rb;
    public bool hasAlerted;
    public float throwThreshold;

    XRGrabInteractable interactable;
    public NPC_DoDoer nPC_DoDoer;
    public bool Once;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        nPC_DoDoer = FindObjectOfType<NPC_DoDoer>();

        rb = GetComponent<Rigidbody>();
        throwThreshold = 7.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (nPC_DoDoer.SmartNPCBrain == true)
        {
            if (Once == false)
            {
                if (interactable.Grabbed == true)
                {
                    if (NPCDoGrabReation.Name != "")
                    {
                        nPC_DoDoer.NewDoIsNeeded(NPCDoGrabReation);

                        Once = true;
                    }
                }
            }

            if (interactable.Dropped == true)
            {
                //Debug.Log(rb.angularVelocity.magnitude);
                //Physic Jakob her
                if (rb.angularVelocity.magnitude > throwThreshold && hasAlerted == false)
                {
                    Debug.Log("You trhwoed an item to hard");

                    nPC_DoDoer.NewDoIsNeeded(nPC_DoDoer.Throw);
                    StartCoroutine(AlertCoolDown());

                    nPC_DoDoer.Temperment += 10;
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
