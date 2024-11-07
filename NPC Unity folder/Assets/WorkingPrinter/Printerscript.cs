using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printerscript : MonoBehaviour
{

    public GameObject Paper;

    public Transform PaperPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player"))
        {
            Paper.transform.position = PaperPosition.position;
            //Move paper to set location
            print("Paper printed :3");
        }
    }
}
