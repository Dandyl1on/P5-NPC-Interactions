using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class CoffeMachineControl : MonoBehaviour
{
    [SerializeField] GameObject coffeStream, waterStream, mugCoffee, mug;
    private bool coffeeMade = false;
    XRGrabInteractable mugScript;
    AudioSource coffeeMachineSound;

    void Start()
    {
        ChangeStreams(false);
        mugCoffee.SetActive(false);
        mugScript = mug.GetComponent<XRGrabInteractable>();
        coffeeMachineSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("mug") && coffeeMade == false)
        {
            StartCoroutine(MakeCoffee());
            coffeeMade = true;
        }
    }

    private void ChangeStreams(bool state)
    {
        coffeStream.SetActive(state);
        waterStream.SetActive(state);
    }

    IEnumerator MakeCoffee()
    {
        ChangeStreams(true);
        mugScript.enabled = false;
        coffeeMachineSound.Play();
        yield return new WaitForSeconds(2);
        ChangeStreams(false);
        mugScript.enabled = true;
        mugCoffee.SetActive(true);
    }
}
