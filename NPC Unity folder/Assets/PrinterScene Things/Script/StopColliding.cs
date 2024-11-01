using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StopColliding : MonoBehaviour
{
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;

    public float Timer;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
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
