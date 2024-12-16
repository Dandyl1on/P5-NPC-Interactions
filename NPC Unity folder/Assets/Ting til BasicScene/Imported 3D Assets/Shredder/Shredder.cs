using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Shredder : MonoBehaviour
{
    [SerializeField] GameObject paperParticles;
    AudioSource paperShredSound;

    public bool YourTurn;


    public GameObject Fire;

    private void Start()
    {
        paperShredSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("paper"))
        {
            Destroy(other.gameObject);
            Instantiate(paperParticles);
            paperShredSound.Play();

            if (YourTurn == true)
            {
                YourTurn = false;

                StartCoroutine(delay());
            }
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2);
        Fire.SetActive(true);
        yield return new WaitForSeconds(1);
        FindObjectOfType<NPCBrain>().MoveOn = true;
    }
}
