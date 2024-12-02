using System.Collections;
using System.Collections.Generic;
using UnityEditor.DeviceSimulation;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class PCMouseScript : MonoBehaviour
{
    public GameObject mouseObject, popup, blueScreen, mailWindow, mailSent, mailDeleted, deleteButton, sendButton;
    public float sensitivity;

    public float maxY, maxX;

    //[SerializeField]GameObject vrControllerHandRight;
    //ControllerAnimator yoinkScript;
    MouseScript yoinkScript2;
    [SerializeField] Animator handAnimator;
    float triggerValue;

    [SerializeField] AudioSource mailSentSound, virusPopupSound, PCdiedSound;

    AudioSource mouseClickSound;
    bool mouseClicked = false;

    bool popupHappened = false;

    public bool YourTurn;

    private void Start()
    {
        //yoinkScript = vrControllerHandRight.GetComponent<ControllerAnimator>();
        yoinkScript2 = mouseObject.GetComponent<MouseScript>();
        mouseClickSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        triggerValue = handAnimator.GetFloat("Trigger");

        Vector3 followPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        followPosition.x = Mathf.Clamp(transform.localPosition.x, transform.localPosition.x - maxX, transform.localPosition.x + maxX);
        followPosition.y = Mathf.Clamp(transform.localPosition.y, transform.localPosition.y - maxY, transform.localPosition.y + maxY);

        transform.localPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        if(yoinkScript2.mouseHeld == true && triggerValue > 0.2f && mouseClicked == false)
        {
            mouseClickSound.Play();
            mouseClicked = true;
        }
        if(triggerValue < 0.2f)
        {
            mouseClicked = false;
        }

        //Debug.Log(yoinkScript.triggerYoinkValue);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("popup"))
        {
            if(yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("popupVirus"))
        {
            if (yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                popup.SetActive(false);
                blueScreen.SetActive(true);
                PCdiedSound.Play();

                if (YourTurn == true)
                {
                    FindObjectOfType<NPC_DoDoer>().MoveOn = true;

                    YourTurn = false;
                }
            }
        }

        if (other.gameObject.CompareTag("mailIcon"))
        {
            if (yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                mailWindow.SetActive(true);
                if(popupHappened == false)
                {
                    StartCoroutine(SpawnAfterTime());
                }             
            }
        }

        if (other.gameObject.CompareTag("mailWindowClose"))
        {
            if (yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                mailWindow.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("mailWindowSend"))
        {
            if (yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                mailSent.SetActive(true);
                mailSentSound.Play();

                deleteButton.SetActive(false);
                sendButton.SetActive(false);

                if (YourTurn == true)
                {
                    FindObjectOfType<NPC_DoDoer>().MoveOn = true;

                    YourTurn = false;
                }
            }
        }

        if (other.gameObject.CompareTag("mailWindowDelete"))
        {
            if (yoinkScript2.mouseHeld == true && triggerValue > 0.2f)
            {
                mailDeleted.SetActive(true);

                deleteButton.SetActive(false);
                sendButton.SetActive(false);

                if (YourTurn == true)
                {
                    FindObjectOfType<NPC_DoDoer>().MoveOn = true;

                    YourTurn = false;
                }
            }
        }
    }

    IEnumerator SpawnAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        popup.SetActive(true);
        virusPopupSound.Play();
    }
}
