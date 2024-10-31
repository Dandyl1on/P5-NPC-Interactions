using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    [SerializeField] GameObject paperParticles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("paper"))
        {
            Destroy(other.gameObject);
            Instantiate(paperParticles);
        }
    }
}
