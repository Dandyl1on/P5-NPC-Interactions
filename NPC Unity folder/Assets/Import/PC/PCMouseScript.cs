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

    [SerializeField]GameObject vrControllerHandRight;
    ControllerAnimator yoinkScript;
    MouseScript yoinkScript2;

    bool popupHappened = false;

    private void Start()
    {
        yoinkScript = vrControllerHandRight.GetComponent<ControllerAnimator>();
        yoinkScript2 = mouseObject.GetComponent<MouseScript>();
    }

    void Update()
    {
        Vector3 followPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        followPosition.x = Mathf.Clamp(transform.localPosition.x, transform.localPosition.x - maxX, transform.localPosition.x + maxX);
        followPosition.y = Mathf.Clamp(transform.localPosition.y, transform.localPosition.y - maxY, transform.localPosition.y + maxY);

        transform.localPosition = new Vector3(mouseObject.transform.localPosition.x * sensitivity, mouseObject.transform.localPosition.z * sensitivity, transform.localPosition.z);

        //Debug.Log(yoinkScript.triggerYoinkValue);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("popup"))
        {
            if(yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
            {
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("popupVirus"))
        {
            if (yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
            {
                popup.SetActive(false);
                blueScreen.SetActive(true);
            }
        }

        if (other.gameObject.CompareTag("mailIcon"))
        {
            if (yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
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
            if (yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
            {
                mailWindow.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("mailWindowSend"))
        {
            if (yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
            {
                mailSent.SetActive(true);

                deleteButton.SetActive(false);
                sendButton.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("mailWindowDelete"))
        {
            if (yoinkScript2.mouseHeld == true && yoinkScript.triggerYoinkValue > 0.2f)
            {
                mailDeleted.SetActive(true);

                deleteButton.SetActive(false);
                sendButton.SetActive(false);
            }
        }
    }

    IEnumerator SpawnAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        popup.SetActive(true);
    }
}
