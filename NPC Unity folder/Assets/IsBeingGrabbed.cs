using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IsBeingGrabbed : MonoBehaviour
{
    public NPCDo NPCDoGrabReation;

    XRGrabInteractable interactable;
    public NPC_DoDoer nPC_DoDoer;
    public bool Once;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        nPC_DoDoer = FindObjectOfType<NPC_DoDoer>();
    }

    // Update is called once per frame
    void Update()
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
    }
}
