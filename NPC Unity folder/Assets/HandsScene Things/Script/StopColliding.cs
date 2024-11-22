using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class StopColliding : MonoBehaviour
{
    XRGrabInteractable interactable;

    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable.Dropped == true)
        {
            int NewLayer = LayerMask.NameToLayer("NotCollideable");
            gameObject.layer = NewLayer;

            if (Timer > 1)
            {
                Timer = 0;
                interactable.Dropped = false;
                NewLayer = LayerMask.NameToLayer("Collideable");
                gameObject.layer = NewLayer;
            }

            Timer += Time.deltaTime;
        }

       
    }
}
