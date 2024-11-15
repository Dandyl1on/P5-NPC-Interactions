using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public Transform mouseCenter;
    public float maxXDistance = 0.5f;
    public float maxZDistance = 0.5f;
    public bool mouseHeld = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("hand"))
        {
            mouseHeld = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("hand"))
        {
            mouseHeld = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("hand"))
        {
            Vector3 newPosition = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);

            newPosition.x = Mathf.Clamp(newPosition.x, mouseCenter.position.x - maxXDistance, mouseCenter.position.x + maxXDistance);
            newPosition.z = Mathf.Clamp(newPosition.z, mouseCenter.position.z - maxZDistance, mouseCenter.position.z + maxZDistance);

            transform.position = newPosition;
        }
    }
}
