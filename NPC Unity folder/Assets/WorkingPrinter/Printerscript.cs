using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Printerscript : MonoBehaviour
{

    public GameObject Paper;
    
    public Transform[] SpawnPositions;
    public Transform Startpos1;
    public Transform Endpos1;
    
    
    private GameObject currentPaper;
    
    public bool isMoving = false;
    public float moveSpeed = 1.0f;
    public float moveProgress = 0.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            moveProgress += Time.deltaTime * moveSpeed;

            currentPaper.transform.position = Vector3.Lerp(Startpos1.position, Endpos1.position, moveProgress );
            
            if (moveProgress >= 1.0f)
            {
                isMoving = false;
                moveProgress = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
        if (other.gameObject.CompareTag("Player") && !isMoving)
        {
            currentPaper = Instantiate(Paper, Startpos1.position, Quaternion.identity);
            
            // int randomIndex = Random.Range(0, SpawnPositions.Length);
            // Vector3 spawnPos = SpawnPositions[randomIndex].position;
            // currentPaper = Instantiate(Paper, spawnPos, Quaternion.identity);
            //Move paper to set location
            print("Paper printed :3");

            isMoving = true;
            moveProgress = 0.0f;
            
        }
    }
}
