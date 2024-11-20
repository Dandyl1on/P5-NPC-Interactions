using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


public class Printerscript : MonoBehaviour
{

    public GameObject Paper;
    public AudioSource Printersound;
    
    public Transform Startpos1;
    public Transform Endpos1;
    public Transform Startpos2;
    public Transform Endpos2;
    public Transform Startpos3;
    public Transform Endpos3;
    
    
    private GameObject currentPaper;
    
    public bool isMoving = false;
    public float moveSpeed;
    public float moveProgress = 0.0f;
    
    public Transform Startpos;
    public Transform Endpos;

    public float soundclip = 9.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveSpeed = 1.0f / soundclip;
            moveProgress += Time.deltaTime * moveSpeed;

            currentPaper.transform.position = Vector3.Lerp(Startpos.position, Endpos.position, moveProgress );
            currentPaper.transform.rotation = Quaternion.Lerp(Startpos.rotation, Endpos.rotation, moveProgress);
            
            if (moveProgress >= 1.0f)
            {
                isMoving = false;
                BoxCollider paperCollider = currentPaper.GetComponent<BoxCollider>();
                Rigidbody paperRb = currentPaper.GetComponent<Rigidbody>();

                paperCollider.enabled = true;
                paperRb.isKinematic = false;
                moveProgress = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player") && !isMoving)
        {
            Paperlocation();
            print("Paper printed :3");

            
            
        }
    }

    private void Paperlocation()
    {
        int randomLocation = Random.Range(0, 3);
        Debug.Log(randomLocation);
        if (randomLocation == 0)
        {
            Printersound = GetComponent<AudioSource>();
            Printersound.Play();
            currentPaper = Instantiate(Paper, Startpos1.position, Quaternion.identity);
            Startpos = Startpos1;
            Endpos = Endpos1;
            isMoving = true;
            moveProgress = 0.0f;
        }

        if (randomLocation == 1)
        {
            Printersound = GetComponent<AudioSource>();
            Printersound.Play();
            currentPaper = Instantiate(Paper, Startpos2.position, Quaternion.identity);
            Startpos = Startpos2;
            Endpos = Endpos2;
            isMoving = true;
            moveProgress = 0.0f;
        }

        if (randomLocation == 2)
        {
            Printersound = GetComponent<AudioSource>();
            Printersound.Play();
            currentPaper = Instantiate(Paper, Startpos3.position, Quaternion.identity);
            Startpos = Startpos3;
            Endpos = Endpos3;
            isMoving = true;
            moveProgress = 0.0f;
        }
    }
}
